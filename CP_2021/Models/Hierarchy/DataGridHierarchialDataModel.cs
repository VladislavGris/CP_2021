using CP_2021.Data;
using CP_2021.Infrastructure.Singletons;
using CP_2021.Infrastructure.Units;
using CP_2021.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CP_2021.Models.Hierarchy
{
    class DataGridHierarchialDataModel
    {

        public DataGridHierarchialDataModel() { Children = new List<DataGridHierarchialDataModel>(); }


        public DataGridHierarchialDataModel Parent { get; set; }
        public DataGridHierarchialData DataManager { get; set; }

        public void AddChildren()
        {
            using (ApplicationContext context = new ApplicationContext())
            {
                foreach (ProductionTaskDB child in context.Production_Plan.Where(t => t.MyParent.Parent == ((ProductionTaskDB)(this.Data))).OrderBy(t => t.MyParent.LineOrder))
                {
                    DataGridHierarchialDataModel childModel = new DataGridHierarchialDataModel() { Data = child, DataManager = this.DataManager };
                    childModel.IsExpanded = child.Expanded;
                    childModel.AddChildren();
                    this.AddChild(childModel);
                }
            }
            Debug.WriteLine(DateTime.Now.ToLongTimeString() + " Children added");
        }

        public void AddChild(DataGridHierarchialDataModel t)
        {
            t.Parent = this;
            Children.Add(t);
        }


        #region LEVEL
        private int _level = -1;
        public int Level
        {
            get
            {
                if (_level == -1)
                {
                    _level = (Parent != null) ? Parent.Level + 1 : 0;
                }
                return _level;
            }
        }

        #endregion
        public bool IsExpanded
        {
            get { return _expanded; }
            set
            {
                if (_expanded != value)
                {
                    _expanded = value;
                    if (_expanded == true)
                        Expand();
                    else
                        Collapse();
                }
            }
        }


        public bool IsVisible
        {
            get { return _visible; }
            set
            {
                if (_visible != value)
                {
                    _visible = value;
                    if (_visible)
                        ShowChildren();
                    else
                        HideChildren();
                }
            }
        }
        public bool HasChildren { get { return Children.Count > 0; } }
        public List<DataGridHierarchialDataModel> Children { get; set; }



        public object Data { get; set; } // the Data (Specify Binding as such {Binding Data.Field})

        public IEnumerable<DataGridHierarchialDataModel> VisibleDescendants
        {
            get
            {
                return Children
                    .Where(x => x.IsVisible)
                    .SelectMany(x => (new[] { x }).Concat(x.VisibleDescendants));
            }
        }



        // Expand Collapse
        private bool _expanded = false;
        private bool _visible = false;
        private void Collapse()
        {
            DataManager.RemoveChildren(this);
            foreach (DataGridHierarchialDataModel d in Children)
                d.IsVisible = false;
        }

        private void Expand()
        {
            DataManager.AddChildren(this);
            foreach (DataGridHierarchialDataModel d in Children)
                d.IsVisible = true;
        }




        // Only if this is Expanded
        private void HideChildren()
        {
            if (IsExpanded)
            {
                // Following Order is Critical
                DataManager.RemoveChildren(this);
                foreach (DataGridHierarchialDataModel d in Children)
                    d.IsVisible = false;
            }
        }
        private void ShowChildren()
        {
            if (IsExpanded)
            {
                // Following Order is Critical
                DataManager.AddChildren(this);
                foreach (DataGridHierarchialDataModel d in Children)
                    d.IsVisible = true;
            }
        }
    }
}
