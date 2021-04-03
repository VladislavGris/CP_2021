using Common.Wpf.Data;
using CP_2021.Data;
using CP_2021.Infrastructure.Commands;
using CP_2021.Models;
using CP_2021.Models.Classes;
using CP_2021.ViewModels.Base;
using System.Collections.Generic;
using System.Linq;

namespace CP_2021.ViewModels
{
    internal class DBTestUserControlViewModel : ViewModelBase
    {

        #region Свойства

        #region People
        private List<DBPerson> _People;

        public List<DBPerson> People
        {
            get => _People;
            set => Set(ref _People, value);
        }
        #endregion

        #region Model
        private TreeGridModel _Model;

        public TreeGridModel Model
        {
            get => _Model;
            set => Set(ref _Model, value);
        }
        #endregion

        #endregion

        #region Команды



        #endregion

        #region Методы

        private void InitPeople()
        {
            using (TestDBContext context = new TestDBContext())
            {
                People = new List<DBPerson>();
                People = context.Persons.ToList();
            }
        }

        private void InitModel()
        {
            Model = new TreeGridModel();

            foreach (DBPerson p in People)
            {
                // Выборка корневых элементов
                if (p.Parent_Id == null)
                {
                    Person root = new Person(p.Id, p.Name, p.Position, p.Parent_Id);
                    // Добваление дочерних элементов
                    if (HasChildren(root))
                    {
                        root.HasChildren = true;
                        AddChildren(root);
                    }
                    // Добавление корневого элемента к модели
                    Model.Add(root);
                }
            }
        }

        private bool HasChildren(Person person)
        {
            foreach(DBPerson p in People)
            {
                if(person.Id == p.Parent_Id)
                {
                    return true;
                }
            }
            return false;
        }

        private void AddChildren(Person person)
        {
            foreach(DBPerson p in People)
            {
                if(person.Id == p.Parent_Id)
                {
                    Person childPerson = new Person(p.Id, p.Name, p.Position, p.Parent_Id);
                    // Рекурсивное добавление дочерних элементов
                    if (HasChildren(childPerson))
                    {
                        childPerson.HasChildren = true;
                        AddChildren(childPerson);
                    }
                    person.Children.Add(childPerson);
                }
            }
        }

        #endregion

        public DBTestUserControlViewModel()
        {
            InitPeople();
            InitModel();
        }
    }
}
