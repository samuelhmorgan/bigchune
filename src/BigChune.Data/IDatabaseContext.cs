using BigChune.Data.Domain; 

namespace BigChune.Data
{
    public interface IDatabaseContext
    {
        IRepository<SoundFile> SoundRepository { get; set; }
    }
     
}
