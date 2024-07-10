using BusinessLogic.Abstraction.VMA.Contract;
using BusinessLogic.Abstraction.VMA.Models;
using BusinessLogic.VMA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VMA.MVVM.ViewModels.Menus
{
    public class ReportsViewModel : ViewModelBase
    {
        public ICommand ExportPaymentNoteCommand { get; }
        public readonly IReportExportToExcelPaymentNote _reportExportToExcelPaymentNote;
        public ReportsViewModel(IReportExportToExcelPaymentNote reportExportToExcelPaymentNote)
        {
            _reportExportToExcelPaymentNote= reportExportToExcelPaymentNote;
            ExportPaymentNoteCommand = new ViewModelAsyncCommand<ExportPaymentNoteData>(ExportPaymentNote);
        }

        private async Task ExportPaymentNote(ExportPaymentNoteData data)
        {
             await _reportExportToExcelPaymentNote.ExportPaymentNotes().ConfigureAwait(true);
        }
    }
}
