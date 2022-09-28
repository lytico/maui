namespace Microsoft.Maui.Devices
{
	partial class BatteryImplementation : IBattery
	{
		void StartEnergySaverListeners() { }

		void StopEnergySaverListeners() { }

		public void StartBatteryListeners() { }

		public void StopBatteryListeners() {}

		public double ChargeLevel
		{
			get
			{
				return 1.0;
			}
		}

		public BatteryState State
		{
			get
			{
				return BatteryState.Full;
			}
		}

		public BatteryPowerSource PowerSource
		{
			get
			{
				return BatteryPowerSource.AC;
			}
		}

		public EnergySaverStatus EnergySaverStatus => EnergySaverStatus.Off;
	}
}
