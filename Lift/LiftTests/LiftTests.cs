using Lift;

namespace LiftTests
{
    public class LiftTests
    {
        private LiftSimulator simulator;

        [SetUp]
        public void Setup()
        {
            this.simulator = new LiftSimulator();
        }

        [TestCase(20, new int[] {0, 0, 0, 0}, new int[] {4, 4, 4, 4})]
        [TestCase(5, new int[] {0, 0, 0}, new int[] {4, 1, 0})]
        [TestCase(15, new int[] {0, 0, 0, 0}, new int[] {4, 4, 4, 3})]
        [TestCase(20, new int[] {0, 2, 0}, new int[] {4, 4, 4})]
        [TestCase(3, new int[] {2, 4, 3}, new int[] {4, 4, 4})]
        public void Test_FitPeopleOnTheLift_ValidInput(int people, int[] cabinsState, int[] expectedCabinsState)
        {
            // Act
            int[] cabins = simulator.FitPeopleOnTheLift(people, cabinsState);

            // Assert
            for (int i = 0; i < cabins.Length; i++)
            {
                Assert.That(cabins[i], Is.EqualTo(expectedCabinsState[i]));
            }
        }

        [TestCase(0, new int[] {0, 0, 0, 0})]
        [TestCase(-10, new int[] {0, 0, 0})]
        public void Test_FitPeopleOnTheLift_InvalidPeopleCount(int people, int[] cabinsState)
        {
            // Assert
            Assert.That(() => simulator.FitPeopleOnTheLift(people, cabinsState), Throws.InstanceOf<ArgumentException>());
        }

        [TestCase(8, new int[] {5, 0, 0})]
        [TestCase(100, new int[] {0, -1, 0})]
        [TestCase(10, new int[] {0, 0, 100})]
        public void Test_FitPeopleOnTheLift_InvalidCabinSize(int people, int[] cabinsState)
        {
            // Assert
            Assert.That(() => simulator.FitPeopleOnTheLift(people, cabinsState), Throws.InstanceOf<ArgumentException>());
        }

        [TestCase(15, new int[] {})]
        [TestCase(10, null)]
        public void Test_FitPeopleOnTheLift_InvalidLiftState(int people, int[] cabinsState)
        {
            // Assert
            Assert.That(() => simulator.FitPeopleOnTheLift(people, cabinsState), Throws.InstanceOf<ArgumentException>());
        }

        [TestCase(20, new int[] {0, 0, 0, 0}, "There isn't enough space! 4 people in a queue!")]
        [TestCase(5, new int[] {4, 1, 0}, "The lift has 2 empty spots!")]
        [TestCase(16, new int[] {0, 0, 0, 0}, "All people placed and the lift if full.")]
        public void FitPeopleOnTheLiftAndGetResult_CheckOutputMessage(int people, int[] cabinsState, string message)
        {
            // Act
            string responseMessage = simulator.FitPeopleOnTheLiftAndGetResult(people, cabinsState);

            // Assert
            Assert.That(responseMessage, Does.Contain(message));
        }
    }
}