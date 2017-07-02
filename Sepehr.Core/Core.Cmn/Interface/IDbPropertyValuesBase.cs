using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn
{
    public interface IDbPropertyValuesBase
    {
        //
        // Summary:
        //     Gets or sets the value of the property with the specified property name. The
        //     value may be a nested instance of this class.
        //
        // Parameters:
        //   propertyName:
        //     The property name.
        //
        // Returns:
        //     The value of the property.
         object this[string propertyName] { get; set; }

        //
        // Summary:
        //     Gets the set of names of all properties in this dictionary as a read-only set.
         IEnumerable<string> PropertyNames { get; }

        //
        // Summary:
        //     Creates a new dictionary containing copies of all the properties in this dictionary.
        //     Changes made to the new dictionary will not be reflected in this dictionary and
        //     vice versa.
        //
        // Returns:
        //     A clone of this dictionary.
         IDbPropertyValuesBase Clone();
        //
      
       
        //
        // Summary:
        //     Gets the value of the property just like using the indexed property getter but
        //     typed to the type of the generic parameter. This is useful especially with nested
        //     dictionaries to avoid writing expressions with lots of casts.
        //
        // Parameters:
        //   propertyName:
        //     Name of the property.
        //
        // Type parameters:
        //   TValue:
        //     The type of the property.
        //
        // Returns:
        //     The value of the property.
         TValue GetValue<TValue>(string propertyName);
        //
        // Summary:
        //     Sets the values of this dictionary by reading values out of the given object.
        //     The given object can be of any type. Any property on the object with a name that
        //     matches a property name in the dictionary and can be read will be read. Other
        //     properties will be ignored. This allows, for example, copying of properties from
        //     simple Data Transfer Objects (DTOs).
        //
        // Parameters:
        //   obj:
        //     The object to read values from.
     
         void SetValues(object obj);
        //
        // Summary:
        //     Sets the values of this dictionary by reading values from another dictionary.
        //     The other dictionary must be based on the same type as this dictionary, or a
        //     type derived from the type for this dictionary.
        //
        // Parameters:
        //   propertyValues:
        //     The dictionary to read values from.
         void SetValues(IDbPropertyValuesBase propertyValues);
        //
        // Summary:
        //     Creates an object of the underlying type for this dictionary and hydrates it
        //     with property values from this dictionary.
        //
        // Returns:
        //     The properties of this dictionary copied into a new object.
         object ToObject();
        
      
    
    }
}
