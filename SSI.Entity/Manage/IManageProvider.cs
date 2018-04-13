namespace SSI.Entity.Manage
{
    public interface IManageProvider
    {
        void AddCurrent(ManageUser user);
        ManageUser Current();
        void EmptyCurrent();
        bool IsOverdue();
    }
}
