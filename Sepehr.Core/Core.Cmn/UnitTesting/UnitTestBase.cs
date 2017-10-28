using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Reflection;

namespace Core.Cmn.UnitTesting
{
    public class UnitTestBase
    {       

        protected static Random _rand = new Random();
        protected static readonly string ALPHA_NUMERICS = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        protected static readonly string[] CULTURES = {
            "fa-IR", "ar-SA", "en-US", "af-ZA", "sq-AL", "ar-DZ", "ar-BH", "ar-EG", "ar-IQ", "ar-JO", "ar-KW", "ar-LB", "ar-LY", "ar-MA", "ar-OM", "ar-QA",
            "ar-SY", "ar-TN", "ar-AE", "ar-YE", "hy-AM", "Cy-az-AZ", "Lt-az-AZ", "eu-ES", "be-BY", "bg-BG", "ca-ES", "zh-CN", "zh-HK", "zh-MO", "zh-SG",
            "zh-TW", "zh-CHS", "zh-CHT", "hr-HR", "cs-CZ", "da-DK", "div-MV", "nl-BE", "nl-NL", "en-AU", "en-BZ", "en-CA", "en-CB", "en-IE", "en-JM", "en-NZ",
            "en-PH", "en-ZA", "en-TT", "en-GB", "en-ZW", "et-EE", "fo-FO", "fi-FI", "fr-BE", "fr-CA", "fr-FR", "fr-LU", "fr-MC", "fr-CH", "gl-ES", "ka-GE",
            "de-AT", "de-DE", "de-LI", "de-LU", "de-CH", "el-GR", "gu-IN", "he-IL", "hi-IN", "hu-HU", "is-IS", "id-ID", "it-IT", "it-CH", "ja-JP", "kn-IN",
            "kk-KZ", "kok-IN", "ko-KR", "ky-KZ", "lv-LV", "lt-LT", "mk-MK", "ms-BN", "ms-MY", "mr-IN", "mn-MN", "nb-NO", "nn-NO", "pl-PL", "pt-BR", "pt-PT",
            "pa-IN", "ro-RO", "ru-RU", "sa-IN", "Cy-sr-SP", "Lt-sr-SP", "sk-SK", "sl-SI", "es-AR", "es-BO", "es-CL", "es-CO", "es-CR", "es-DO", "es-EC", "es-SV",
            "es-GT", "es-HN", "es-MX", "es-NI", "es-PA", "es-PY", "es-PE", "es-PR", "es-ES", "es-UY", "es-VE", "sw-KE", "sv-FI", "sv-SE", "syr-SY", "ta-IN",
            "tt-RU", "te-IN", "th-TH", "tr-TR", "uk-UA", "ur-PK", "Cy-uz-UZ", "Lt-uz-UZ", "vi-VN"
        };

        protected int CultureCount
        {
            get
            {
                return CULTURES.Length;
            }
        }

        protected bool GetRandomBoolean()
        {
            return (_rand.Next() % 2) == 1 ? true : false;
        }

        protected bool? GetRandomBooleanNullable()
        {
            bool? result = null; 
            if (GetRandomBoolean() == true)
            {
                result = GetRandomBoolean();
            }
            return result;
        }

        protected byte GetRandomByte(byte min = byte.MinValue, byte max = byte.MaxValue)
        {
            return (byte)(_rand.Next() % (max - min + 1) + min);
        }

        protected byte? GetRandomByteNullable(byte min = 1, byte max = byte.MaxValue)
        {
            byte? result = null;
            if (GetRandomBoolean() == true)
            {
                result = GetRandomByte();
            }
            return result;
        }

        protected short GetRandomShort(short min = 1, short max = short.MaxValue)
        {
            return (short)(_rand.Next() % (max - min + 1) + min);
        }

        protected short? GetRandomShortNullable(short min = 1, short max = short.MaxValue)
        {
            short? result = null;
            if (GetRandomBoolean() == true)
            {
                result = GetRandomShort();
            }
            return result;
        }

        //[ClassInitialize]
        //public virtual void Initialize(TestContext testContext)
        //{
        //    Core.Cmn.AppBase.StartApplication();
        //}

        protected int GetRandomInt(int min = 1, int max = int.MaxValue)
        {
            return _rand.Next() % (max - min + 1) + min;
        }

        protected long GetRandomLong(long min = 1, long max = long.MaxValue)
        {
            return (_rand.Next()) % (max - min + 1) + min;
        }

        protected int? GetRandomIntNullable(int min = 1, int max = int.MaxValue)
        {
            int? result = null;
            if (GetRandomBoolean() == true)
            {
                result = GetRandomInt();
            }
            return result;
        }

        protected long? GetRandomLongNullable(long min = 1, long max = long.MaxValue)
        {
            long? result = null;
            if (GetRandomBoolean() == true)
            {
                result = GetRandomLong();
            }
            return result;
        }

        protected double GetRandomDouble()
        {
            return _rand.NextDouble();
        }

        protected double? GetRandomDoubleNullable()
        {
            double? result = null;
            if (GetRandomBoolean() == true)
            {
                result = GetRandomDouble();
            }
            return result;
        }

        protected decimal GetRandomDecimal()
        {
            return (decimal)_rand.NextDouble();
        }

        protected decimal? GetRandomDecimalNullable()
        {
            decimal? result = null;
            if (GetRandomBoolean() == true)
            {
                result = GetRandomDecimal();
            }
            return result;
        }

        protected Guid GetRandomGuid()
        {
            return Guid.NewGuid();
        }

        protected Guid? GetRandomGuidNullable()
        {
            Guid? result = null;
            if (GetRandomBoolean() == true)
            {
                result = GetRandomGuid();
            }
            return result;
        }
        protected byte[] GetRandomBytes()
        {
            int size = GetRandomInt(5, 1000);
            byte[] result = new byte[GetRandomInt(5, 1000)];
            for(int i=0; i<result.Length; i++)
            {
                result[i] = GetRandomByte();
            }
            return result;
        }

        protected string GetRandomString(int minLength = 5, int maxLength = 50)
        {
            String result = "";

            int len = _rand.Next() % (maxLength - minLength + 1) + minLength;
            for (int i = 0; i < len; i++)
            {
                result += ALPHA_NUMERICS[_rand.Next() % ALPHA_NUMERICS.Length];
            }
            return result;
        }

        protected DateTime GetRandomDateTime()
        {            
            int year = _rand.Next() % 15 + 2000; 
            int month = _rand.Next() % 12 + 1; 
            int day = _rand.Next() % 28 + 1;

            return new DateTime(year, month, day);            
        }
        protected DateTime? GetRandomDateTimeNullable()
        {
            DateTime? result = null;
            if (GetRandomBoolean() == true)
            {
                result = GetRandomDateTime();
            }
            return result;
        }

        public object GetRandomValue(Type type)
        {
            object value = null;

            if (type == typeof(string))
            {
                value = GetRandomString();
            }
            else if (type == typeof(byte))
            {
                value = GetRandomByte();
            }
            else if (type == typeof(byte?) || type == typeof(Nullable<byte>))
            {
                value = GetRandomByteNullable();
            }
            else if (type == typeof(short))
            {
                value = GetRandomShort();
            }
            else if (type == typeof(short?) || type == typeof(Nullable<short>))
            {
                value = GetRandomShortNullable();
            }
            else if (type == typeof(int))
            {
                value = GetRandomInt(1, 1000);
            }
            else if (type == typeof(long))
            {
                value = GetRandomLong(1, 1000);
            }
            else if (type == typeof(int?) || type == typeof(Nullable<int>))
            {
                value = GetRandomIntNullable(1, 1000);
            }
            else if (type == typeof(long?) || type == typeof(Nullable<long>))
            {
                value = GetRandomIntNullable(1, 1000);
            }
            else if (type == typeof(double))
            {
                value = GetRandomDouble();
            }
            else if (type == typeof(double?))
            {
                value = GetRandomDoubleNullable();
            }
            else if (type == typeof(decimal))
            {
                value = GetRandomDecimal();
            }
            else if (type == typeof(decimal?))
            {
                value = GetRandomDecimalNullable();
            }
            else if (type == typeof(bool))
            {
                value = GetRandomBoolean();
            }
            else if (type == typeof(bool?))
            {
                value = GetRandomBooleanNullable();
            }
            else if (type == typeof(Guid))
            {
                value = GetRandomGuid();
            }
            else if (type == typeof(Guid?))
            {
                value = GetRandomGuidNullable();
            }
            else if (type == typeof(byte[]))
            {
                value = GetRandomBytes();
            }

            return value;
        }
        protected string GetRandomCulture()
        {
            return CULTURES[_rand.Next() % CULTURES.Length];
        }

        protected TEntity BuildSampleEntity<TEntity>() where TEntity : EntityBase<TEntity>, new()
        {
            TEntity entity = new TEntity();
            PropertyInfo[] properties = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (PropertyInfo property in properties)
            {
                if (property.GetSetMethod() != null)
                {
                    object value;

                    Type propertyType = property.PropertyType;

                    if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        value = GetRandomValue(Nullable.GetUnderlyingType(propertyType));
                    }
                    else
                    {
                        value = GetRandomValue(propertyType);
                    }                   

                    property.SetValue(entity, value);
                }
            }
            return entity;
        }
    }
}
