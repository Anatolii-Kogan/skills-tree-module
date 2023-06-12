using System;
using System.Collections.Generic;

namespace Skills.Core
{
    public static class ReferenceProvider
    {
        private static Dictionary< Type, object > _map = new Dictionary< Type, object >();
        
        public static void Register<T>( T reference  ) where T : class
        {
            if( reference == null )
            {
                throw new ArgumentNullException( nameof( reference ) );
            }

            Type type = reference.GetType();
            if (_map.ContainsKey(type))
            {
                _map.Remove(type);
            }

            _map.Add( type, reference );
        }


        public static T GetWithGeneration<T>() where T : ApplicationService, new()
        {
            if (_map.TryGetValue(typeof( T ), out object value ))
            {
                return (T)value;
            }
            
            Register(new T());
            return GetReference<T>();
        }

        public static T GetReference<T>()
        {
            try
            {
                return (T)_map[typeof(T)];
            }
            catch( KeyNotFoundException )
            {
                throw new InvalidOperationException( $@"Service for type {typeof( T ).Name} not exit" );
            }
            catch( InvalidCastException )
            {
                throw new InvalidOperationException( $@"Service has not type {typeof( T ).Name} real type is {_map[ typeof( T ) ].GetType().Name}" );
            }
        }
    }
}
