using NUnit.Framework;
using CrazyWeasol.Crash;
namespace CrazyWeasol.Tests { public class CoreStateTests { [Test] public void StableStateIsNotFractured(){Assert.That(new CoreState(80f).IsFractured,Is.False);} [Test] public void FortyStabilityIsFractured(){Assert.That(new CoreState(40f).IsFractured,Is.True);} [Test] public void ZeroStabilityIsTotalCrash(){Assert.That(new CoreState(0f).IsTotalCrash,Is.True);} } }
