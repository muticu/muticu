using osu.Framework.Testing;

namespace Mutruc.Game.Tests.Visual
{
    public class MutrucTestScene : TestScene
    {
        protected override ITestSceneTestRunner CreateRunner() => new MutrucTestSceneTestRunner();

        private class MutrucTestSceneTestRunner : MutrucGameBase, ITestSceneTestRunner
        {
            private TestSceneTestRunner.TestRunner runner;

            protected override void LoadAsyncComplete()
            {
                base.LoadAsyncComplete();
                Add(runner = new TestSceneTestRunner.TestRunner());
            }

            public void RunTestBlocking(TestScene test) => runner.RunTestBlocking(test);
        }
    }
}
