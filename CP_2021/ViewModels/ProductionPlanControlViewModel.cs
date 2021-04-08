using CP_2021.Data;
using CP_2021.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.ViewModels
{
    class ProductionPlanControlViewModel : ViewModelBase
    {

        #region Свойства

        #endregion

        #region Команды

        #endregion

        #region Методы

        #endregion

        public ProductionPlanControlViewModel()
        {
            #region Команды

            #endregion
            using(ProductionDBContext context = new ProductionDBContext())
            {
            }
        }
    }
}
