using Domain.Enums;

namespace Domain.Entities.Base
{
    public abstract class BaseEntity
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public RecordStatus RecordStatus { get; private set; } = RecordStatus.Enabled;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; protected set; } = DateTime.UtcNow;

        // Methods to transition state
        public void Enable()
        {
            if (RecordStatus == RecordStatus.Enabled)
                throw new InvalidOperationException("Entity is already enabled.");

            RecordStatus = RecordStatus.Enabled;
            UpdateTimestamp();
        }

        public void Disable()
        {
            if (RecordStatus == RecordStatus.Disabled)
                throw new InvalidOperationException("Entity is already disabled.");

            RecordStatus = RecordStatus.Disabled;
            UpdateTimestamp();
        }

        public void SoftDelete()
        {
            if (RecordStatus == RecordStatus.Deleted)
                throw new InvalidOperationException("Entity is already deleted.");

            RecordStatus = RecordStatus.Deleted;
            UpdateTimestamp();
        }

        protected void UpdateTimestamp()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
