using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace MediaJigsaw.Infrastructure
{
    [TypeDescriptionProvider(typeof(CommandMap.CommandMapDescriptionProvider))]
    public class CommandMap
    {
        // Fields
        private Dictionary<string, ICommand> _commands;

        // Methods
        public void AddCommand(string commandName, Action<object> executeMethod)
        {
            this.Commands[commandName] = new DelegateCommand(executeMethod);
        }

        public void AddCommand(string commandName, Action<object> executeMethod, Predicate<object> canExecuteMethod)
        {
            this.Commands[commandName] = new DelegateCommand(executeMethod, canExecuteMethod);
        }

        public void RemoveCommand(string commandName)
        {
            this.Commands.Remove(commandName);
        }

        // Properties
        protected Dictionary<string, ICommand> Commands
        {
            get
            {
                if (null == this._commands)
                {
                    this._commands = new Dictionary<string, ICommand>();
                }
                return this._commands;
            }
        }

        // Nested Types
        private class CommandMapDescriptionProvider : TypeDescriptionProvider
        {
            // Methods
            public CommandMapDescriptionProvider()
                : this(TypeDescriptor.GetProvider(typeof(CommandMap)))
            {
            }

            private CommandMapDescriptionProvider(TypeDescriptionProvider parent)
                : base(parent)
            {
            }

            public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
            {
                return new CommandMap.CommandMapDescriptor(base.GetTypeDescriptor(objectType, instance), instance as CommandMap);
            }
        }

        private class CommandMapDescriptor : CustomTypeDescriptor
        {
            // Fields
            private CommandMap _map;

            // Methods
            public CommandMapDescriptor(ICustomTypeDescriptor descriptor, CommandMap map)
                : base(descriptor)
            {
                this._map = map;
            }

            public override PropertyDescriptorCollection GetProperties()
            {
                PropertyDescriptor[] props = new PropertyDescriptor[this._map.Commands.Count];
                int pos = 0;
                foreach (KeyValuePair<string, ICommand> command in this._map.Commands)
                {
                    props[pos++] = new CommandMap.CommandPropertyDescriptor(command);
                }
                return new PropertyDescriptorCollection(props);
            }
        }

        private class CommandPropertyDescriptor : PropertyDescriptor
        {
            // Fields
            private ICommand _command;

            // Methods
            public CommandPropertyDescriptor(KeyValuePair<string, ICommand> command)
                : base(command.Key, null)
            {
                this._command = command.Value;
            }

            public override bool CanResetValue(object component)
            {
                return false;
            }

            public override object GetValue(object component)
            {
                CommandMap map = component as CommandMap;
                if (null == map)
                {
                    throw new ArgumentException("component is not a CommandMap instance", "component");
                }
                return map.Commands[this.Name];
            }

            public override void ResetValue(object component)
            {
                throw new NotImplementedException();
            }

            public override void SetValue(object component, object value)
            {
                throw new NotImplementedException();
            }

            public override bool ShouldSerializeValue(object component)
            {
                return false;
            }

            // Properties
            public override Type ComponentType
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public override bool IsReadOnly
            {
                get
                {
                    return true;
                }
            }

            public override Type PropertyType
            {
                get
                {
                    return typeof(ICommand);
                }
            }
        }

        private class DelegateCommand : ICommand
        {
            // Fields
            private Predicate<object> _canExecuteMethod;
            private Action<object> _executeMethod;

            // Events
            public event EventHandler CanExecuteChanged
            {
                add
                {
                    CommandManager.RequerySuggested += value;
                }
                remove
                {
                    CommandManager.RequerySuggested -= value;
                }
            }

            // Methods
            public DelegateCommand(Action<object> executeMethod)
                : this(executeMethod, null)
            {
            }

            public DelegateCommand(Action<object> executeMethod, Predicate<object> canExecuteMethod)
            {
                if (null == executeMethod)
                {
                    throw new ArgumentNullException("executeMethod");
                }
                this._executeMethod = executeMethod;
                this._canExecuteMethod = canExecuteMethod;
            }

            public bool CanExecute(object parameter)
            {
                return ((this._canExecuteMethod == null) || this._canExecuteMethod(parameter));
            }

            public void Execute(object parameter)
            {
                this._executeMethod(parameter);
            }
        }
    }
}
