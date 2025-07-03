using HypeLab.IO.Core.Exceptions;
using HypeLab.IO.Core.Helpers;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace HypeLab.IO.Core.Factories
{
    /// <summary>
    /// Provides functionality to create and initialize instances of a specified type.
    /// </summary>
    /// <remarks>This factory class is designed to simplify the creation of objects by dynamically selecting a
    /// suitable constructor and initializing properties with specified values. It supports both public constructors and
    /// property initialization for writable or `init`-only properties. Errors during property assignment are logged if
    /// a logger is provided.</remarks>
    public static class GenericObjectFactory
    {
        /// <summary>
        /// Creates an instance of the specified type <typeparamref name="T"/> and initializes its properties with the
        /// provided values.
        /// </summary>
        /// <remarks>The method attempts to create an instance of <typeparamref name="T"/> by prioritizing
        /// public constructors  whose parameters match the provided property names and types. If no suitable
        /// constructor is found,  it falls back to using <see cref="Activator.CreateInstance(Type, bool)"/> to create
        /// the instance.  After the instance is created, all writable properties (or properties with `init` accessors) 
        /// in the <paramref name="propertyValues"/> dictionary are set to the specified values.  Any errors during
        /// property assignment are logged (if a logger is provided) but do not stop the process.</remarks>
        /// <typeparam name="T">The type of the object to create. Must be a reference type.</typeparam>
        /// <param name="propertyValues">A dictionary containing property-value pairs to initialize the created instance.  The keys represent the
        /// properties to set, and the values represent the corresponding values to assign.</param>
        /// <param name="logger">An optional <see cref="ILogger"/> instance for logging errors or warnings during the creation process. If
        /// null, no logging will occur.</param>
        /// <returns>An instance of type <typeparamref name="T"/> with its properties initialized to the specified values.</returns>
        /// <exception cref="ObjectCreationException">Thrown if an instance of type <typeparamref name="T"/> cannot be created due to missing suitable
        /// constructors  or if the type is abstract or an interface.</exception>
        /// <exception cref="NullObjectException">Thrown if the creation process results in a null instance of type <typeparamref name="T"/>.</exception>
        public static T Create<T>(Dictionary<PropertyInfo, object?> propertyValues, ILogger? logger = null)
            where T : class
        {
            Type type = typeof(T);
            T? instance = null;

            // priorità ai costruttori pubblici con parametri che corrispondono alle proprietà fornite
            foreach (ConstructorInfo ctor in type.GetConstructors(BindingFlags.Public | BindingFlags.Instance).OrderByDescending(c => c.GetParameters().Length))
            {
                ParameterInfo[] parameters = ctor.GetParameters();
                bool canUse = parameters
                    .All(p => propertyValues.Keys.Any(prop => string.Equals(prop.Name, p.Name, StringComparison.OrdinalIgnoreCase) && (p.ParameterType == prop.PropertyType || p.ParameterType.IsAssignableFrom(prop.PropertyType))));

                if (!canUse)
                    continue;

                try
                {
                    object?[] args = [.. parameters.Select(p =>
                    {
                        PropertyInfo prop = propertyValues.Keys.First(propz => string.Equals(propz.Name, p.Name, StringComparison.OrdinalIgnoreCase));
                        return propertyValues[prop];
                    })];

                    instance = (T)ctor.Invoke(args);
                    break; // trovato un costruttore valido, passo alla valorizzazione extra
                }
                catch (Exception ex)
                {
                    logger?.LogError(ex, "Constructor {CtorName} failed for type {TypeFullName}. Trying next constructor.", ctor.Name, type.FullName);
                    instance = null; // resetta l'istanza per il prossimo ciclo
                }
            }

            // fallback se nessun costruttore è adatto
            if (instance == null)
            {
                try
                {
                    instance = (T?)Activator.CreateInstance(type, nonPublic: true);
                }
                catch (Exception ex)
                {
                    string msg = $"Unable to create an instance of type {type.FullName}. No suitable constructor found or type is abstract/interface.";
                    logger?.LogError(ex, "{Msg}", msg);
                    throw new ObjectCreationException(msg, ex);
                }

                // se ancora null, zi vidì
                if (instance == null)
                {
                    string msg = $"Failed to create an instance of type {type.FullName}. The instance is null.";
                    logger?.LogError("{Msg}", msg);
                    throw new NullObjectException(msg);
                }
            }

            // assegna tutte le proprietà compatibili con un setter
            foreach (KeyValuePair<PropertyInfo, object?> propVal in propertyValues)
            {
                if (propVal.Key.CanWrite || propVal.Key.SetMethod?.IsInitOnly() == true)
                {
                    try
                    {
                        propVal.Key.SetValue(instance, propVal.Value);
                    }
                    catch (Exception ex)
                    {
                        logger?.LogError(ex, "Failed to set property '{PropertyName}' on type {TypeFullName}. Property may not be writable or value is incompatible.", propVal.Key.Name, type.FullName);
                        // ignora errori di assegnazione, ma logga l'errore
                    }
                }
            }

            return instance;
        }
    }
}
