using Moq;
using Serilog;

namespace DAB.CharityBox.Test.Mock
{
    public class MockLogger
    {
        public ILogger GetMockedLogger<T>()
        {
            var moq = new Mock<ILogger>();
            return moq.Object;
        }
    }
}
