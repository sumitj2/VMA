using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMA.MVVM.ViewModels
{
    public class ViewModelBase : IDataErrorInfo, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string? this[string columnName]
        {
            get
            {
                var validationResults = new List<ValidationResult>();
                var context = new ValidationContext(this) { MemberName = columnName };
                Validator.TryValidateProperty(
                    GetType().GetProperty(columnName)?.GetValue(this),
                    context,
                    validationResults);

                return validationResults.Any() ? validationResults.First().ErrorMessage : string.Empty;
            }
        }
        public string Error => null;
    }
}
