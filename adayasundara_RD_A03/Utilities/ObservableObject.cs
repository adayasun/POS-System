/*
*	FILE			:	ObervableObjects.cs
*	PROJECT			:	PROG2111 - Relational Databases
*	PROGRAMMER		:	Amy Dayasundara
*	FIRST VERSION	:	2019 - 11 - 30
*	DESCRIPTION		:	
*	                    This class is taken from a youtube video which assists in the client
*	                    property changes
*/

#region Systems
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
#endregion Systems

namespace adayasundara_RD_A03.Utilities
{
    /*
     * NAME: ObservableObject
     * 
     * PURPOSE: Handles the events of the property changes for the view and view model
     * 
     */

    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Raises an event to alert client to changed property value
        /// </summary>
        /// <param name="propertyName"></param> <b>string</b> - name of property which has been changed
        // referenced from https://www.youtube.com/watch?v=CQYvjlDoJ08&t=21s
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        ///     Raises an event to alert client to changed property value. Compares new value to old
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="backingField"></param> <b>ref T backingField</b> - field to change
        /// <param name="value"></param> <b>T value</b> - new value to apply to changed field
        /// <param name="propertyName"></param> <b>string</b> - name of property which has been changed
        /// <returns>bool true - if values </returns>
        // referenced from https://www.youtube.com/watch?v=CQYvjlDoJ08&t=21s
        protected virtual bool OnPropertyChanged<T>(ref T backingField, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingField, value))
                return false;

            backingField = value;
            OnPropertyChanged(propertyName);
            return true;
        }

    }
}
