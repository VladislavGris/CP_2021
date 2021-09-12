using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Models.Hierarchy
{
    class DataGridHierarchialData : ObservableCollection<DataGridHierarchialDataModel>
    {
        public List<DataGridHierarchialDataModel> RawData { get; set; }
        public DataGridHierarchialData() { RawData = new List<DataGridHierarchialDataModel>(); }

        public void Initialize()
        {
            this.Clear();
            foreach (DataGridHierarchialDataModel m in RawData.Where(c => c.IsVisible).SelectMany(x => new[] { x }.Concat(x.VisibleDescendants)))
            {
                this.Add(m);
            }
        }

        public void AddChildren(DataGridHierarchialDataModel d)
        {
            if (!this.Contains(d))
                return;
            int parentIndex = this.IndexOf(d);
            foreach (DataGridHierarchialDataModel c in d.Children)
            {
                parentIndex += 1;
                this.Insert(parentIndex, c);
            }
        }

        public void RemoveChildren(DataGridHierarchialDataModel d)
        {
            foreach (DataGridHierarchialDataModel c in d.Children)
            {
                if (this.Contains(c))
                    this.Remove(c);
            }
        }
    }
}
