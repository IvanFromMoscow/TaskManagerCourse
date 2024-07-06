namespace TaskManagerCourse.Api.Services
{
    public abstract class AbstarctService
    {
        public bool DoAction(Action action)
        {
            try
            {
                action.Invoke();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
