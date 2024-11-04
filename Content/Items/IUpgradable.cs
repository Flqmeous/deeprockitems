using deeprockitems.Content.Upgrades;

namespace deeprockitems.Content.Items
{
    public interface IUpgradable
    {
        public UpgradeList UpgradeMasterList { get; set; }
        public abstract void ApplyStatUpgrades();
        public void ResetStats() { }
        /// <summary>
        /// Initializes the master list of upgrades this weapon will use.
        /// </summary>
        /// <returns></returns>
        public UpgradeList InitializeUpgrades();
    }
}
