using System.Text.Json;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{

    abstract class DalObject
    {
        public const string IDColumnName = "ID";
        protected DalControl _controller;
        public int Id { get; set; } = -1;
        protected DalObject(DalControl controller)
        {
            _controller = controller;
        }
    }
}
