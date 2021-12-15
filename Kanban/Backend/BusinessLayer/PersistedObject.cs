using IntroSE.Kanban.Backend.DataAccessLayer;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    interface PersistedObject<T> where T : DalObject
    {
        //converting the object with T type to DalObject type.
        T todal();

        //converting the object with DalObject type to T type.
        void import(T dal);

        //Save has two options:-
        //1) action = "insert" inserting the new object to the database.
        //2) action = "delete" deleting the existed object from the database.
        void Save(string action);
    }
}
