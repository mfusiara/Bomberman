using System;
using System.Collections.Generic;
using System.Linq;

namespace GameLibrary.Controls.Selectors
{
    public class SelectionGroupManager
    {
        private IList<CheckBox> _selectors;

        public SelectionGroupManager()
        {
            _selectors = new List<CheckBox>();
        }

        public void Add(CheckBox checkBox)
        {
            _selectors.Add(checkBox);
            if (_selectors.Count == 1) _selectors.First().IsChecked = true;
            checkBox.SelectionChange += ChangeSelection;
        }

        private void ChangeSelection(object sender, EventArgs e)
        {
            var selector = sender as CheckBox;
            if (!selector.IsChecked)
            {
                if (_selectors.All(s => !s.IsChecked)) _selectors.First().IsChecked = true;
                return;
            }
            

            foreach (var checkBox in _selectors.Where(s => s != selector))
            {
                if(checkBox.IsChecked) checkBox.IsChecked = false;
            }
        }
    }
}