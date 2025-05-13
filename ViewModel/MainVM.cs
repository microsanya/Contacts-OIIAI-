using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Linq;
using View.Model;
using View.Model.Services;

namespace View.ViewModel
{
    public class MainVM : INotifyPropertyChanged
    {
        private Contact _originalContact;
        private Contact _editableContact;
        private bool _isReadOnlyMode = true;

        private bool _isChangingContact = false;

        private bool _hasChangesNotApplied = false;
        private bool _isAddingNewContact = false;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand RemoveCommand { get; }
        public ICommand ApplyCommand { get; }

        public ObservableCollection<Contact> Contacts { get; set; }

        public MainVM()
        {
            ContactSerializer.CreateDirectory();
            Contacts = new ObservableCollection<Contact>(ContactSerializer.LoadContact());
            AddCommand = new RelayCommand(AddContact, CanAddContact);
            EditCommand = new RelayCommand(EditContact, CanEditContact);
            RemoveCommand = new RelayCommand(RemoveContact, CanRemoveContact);
            ApplyCommand = new RelayCommand(ApplyContact, CanApplyContact);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Contact CurrentContact
        {
            get => _editableContact;
            set
            {

                if (_editableContact != null && !IsReadOnlyMode)
                {   
                    CancelEdit();
                }

                _editableContact = value;
                OnPropertyChanged(nameof(IsAddOrEditMode));
                OnPropertyChanged(nameof(CurrentContact));
                OnPropertyChanged(nameof(IsContactSelected));

                /*if (_isChangingContact || _editableContact == value)
                    return;

                _isChangingContact = true;

                if (_editableContact != null && !IsContactValid(_editableContact))
                {
                    CancelEdit();
                    _isChangingContact = false;
                    return;
                }

                if ((_hasChangesNotApplied || _isAddingNewContact) && _editableContact != null)
                {
                    CancelEdit();
                    _hasChangesNotApplied = false;
                    _isAddingNewContact = false;
                }

                _originalContact = value;
                _editableContact = value != null ? (Contact)value.Clone() : null;

                OnPropertyChanged(nameof(CurrentContact));
                OnPropertyChanged(nameof(IsContactSelected));
                OnPropertyChanged(nameof(IsAddOrEditMode));

                _isChangingContact = false;*/
            }
        }


        private bool IsContactValid(Contact contact)
        {
            if (contact == null) return true;

            var nameError = contact[nameof(Contact.Name)];
            var phoneError = contact[nameof(Contact.PhoneNumber)];
            var emailError = contact[nameof(Contact.Email)];

            return string.IsNullOrEmpty(nameError) && string.IsNullOrEmpty(phoneError) && string.IsNullOrEmpty(emailError);
        }

        public bool IsAddOrEditMode => !IsReadOnlyMode;

        public bool IsReadOnlyMode
        {
            get => _isReadOnlyMode;
            set
            {
                _isReadOnlyMode = value;
                OnPropertyChanged(nameof(IsReadOnlyMode));
                OnPropertyChanged(nameof(IsAddOrEditMode));
            }
        }

        public bool IsContactSelected => _editableContact != null;

        public void EditContact(object parameter)
        {
            _originalContact = (Contact)CurrentContact.Clone();
            IsReadOnlyMode = false;
            /*if (_originalContact != null)
            {
                _editableContact = new Contact(_originalContact.Name, _originalContact.PhoneNumber, _originalContact.Email);
                CurrentContact = _originalContact; 
                IsReadOnlyMode = false;
                _hasChangesNotApplied = true;
            }*/
        }


        public void RemoveContact(object parameter)
        {
            if (CurrentContact == null)
            {
                return;
            }

            int index = Contacts.IndexOf(CurrentContact);
            Contacts.Remove(CurrentContact);

            if (Contacts.Any())
            {
                CurrentContact = index < Contacts.Count ? Contacts[index] : Contacts.Last();
            }
            else
            {
                CurrentContact = null;
            }

            ContactSerializer.SaveContact(Contacts);
        }

        public void AddContact(object parameter)
        {
            /*_originalContact = null;*/
            CurrentContact = null;
            CurrentContact = new Contact();
            IsReadOnlyMode = false;
            _isAddingNewContact = true;
            OnPropertyChanged(nameof(CurrentContact));
            OnPropertyChanged(nameof(IsContactSelected));
        }

        public void ApplyContact(object parameter)
        {
            if (parameter is not BindingGroup bindingGroup)
            {
                return;
            }

            bindingGroup.CommitEdit();

            if (_editableContact == null || !IsContactValid(_editableContact))
            {
                CancelEdit();
                return;
            }

            if (_originalContact != null)
            {
                int index = Contacts.IndexOf(_originalContact);
                if (index != -1)
                {
                    Contacts[index] = _editableContact;
                    _originalContact = _editableContact;
                }
            }
            else
            {
                Contacts.Add(_editableContact);
            }

            IsReadOnlyMode = true;
            ContactSerializer.SaveContact(Contacts);
            _hasChangesNotApplied = false;

            _hasChangesNotApplied = false;

            CurrentContact = _editableContact;

            _editableContact = null;

            OnPropertyChanged(nameof(Contacts));
            OnPropertyChanged(nameof(CurrentContact));
        }



        private void CancelEdit()
        {
            if (_originalContact != null)
            {
                CurrentContact.Name = _originalContact.Name;
                CurrentContact.PhoneNumber = _originalContact.PhoneNumber;
                CurrentContact.Email = _originalContact.Email;
            }

            IsReadOnlyMode = true;
            OnPropertyChanged(nameof(IsReadOnlyMode));
            OnPropertyChanged(nameof(IsAddOrEditMode));
            /*_editableContact = _originalContact != null ? (Contact)_originalContact.Clone() : null;
            _hasChangesNotApplied = false;

            CurrentContact = null;
            CurrentContact = _editableContact;
            RefreshCurrentContact();

            IsReadOnlyMode = true;

            _hasChangesNotApplied = false;*/
        }

        private void RefreshCurrentContact()
        {
            var temp = _editableContact;
            _editableContact = null;
            OnPropertyChanged(nameof(CurrentContact));

            _editableContact = temp;
            OnPropertyChanged(nameof(CurrentContact));
        }

        private bool CanAddContact(object parameter) => !IsAddOrEditMode;

        private bool CanEditContact(object parameter) => IsContactSelected && !IsAddOrEditMode;

        private bool CanRemoveContact(object parameter) => IsContactSelected && !IsAddOrEditMode;

        private bool CanApplyContact(object parameter) => IsAddOrEditMode && !HasValidationErrors;

        private bool HasValidationErrors => _editableContact != null &&
                                            (!string.IsNullOrEmpty(_editableContact[nameof(Contact.Name)]) ||
                                             !string.IsNullOrEmpty(_editableContact[nameof(Contact.PhoneNumber)]) ||
                                             !string.IsNullOrEmpty(_editableContact[nameof(Contact.Email)]));
    }

}
