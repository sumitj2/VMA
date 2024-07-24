using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Abstraction.VMA.Models
{
    public class SearchModel
    {
        private int _searchId;

        public int SearchId
        {
            get { return _searchId; }
            set { _searchId = value; }
        }
        private string? _nameSearch;

        public string? NameSearch
        {
            get { return _nameSearch; }
            set { _nameSearch = value; }
        }
    }
}
