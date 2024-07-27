using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
using Serilog;
using System.Reflection;
using System.Windows.Input;

namespace VMA.MVVM.ViewModels.Menus
{
    public class ReportsViewModel : ViewModelBase
    {
        public ICommand ExportPaymentNoteCommand { get; }
        public readonly IReportExportToExcelPaymentNote _reportExportToExcelPaymentNote;
        public ReportsViewModel(IReportExportToExcelPaymentNote reportExportToExcelPaymentNote)
        {
            Log.Logger.Information(string.Format("Class: {0}, Method: {1} - Into the constructor", this.GetType().Name, MethodBase.GetCurrentMethod().Name));

            _reportExportToExcelPaymentNote = reportExportToExcelPaymentNote;
            ExportPaymentNoteCommand = new ViewModelAsyncCommand<ExportPaymentNoteData>(ExportPaymentNote);
        }

        private async Task ExportPaymentNote(ExportPaymentNoteData data)
        {
             await _reportExportToExcelPaymentNote.ExportPaymentNotes().ConfigureAwait(true);
        }
    }
}
