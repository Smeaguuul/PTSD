﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by Reqnroll (https://www.reqnroll.net/).
//      Reqnroll Version:2.0.0.0
//      Reqnroll Generator Version:2.0.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
using Reqnroll;
namespace Tests.Features
{
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Reqnroll", "2.0.0.0")]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class MatchPointsFeature
    {
        
        private global::Reqnroll.ITestRunner testRunner;
        
        private Microsoft.VisualStudio.TestTools.UnitTesting.TestContext _testContext;
        
        private static string[] featureTags = ((string[])(null));
        
        private static global::Reqnroll.FeatureInfo featureInfo = new global::Reqnroll.FeatureInfo(new global::System.Globalization.CultureInfo("en-US"), "Features", "MatchPoints", "A feature that tests the method that keeps tracks of the point logic. \r\nStuff lik" +
                "e: When to move on to a new set? When has a team won? And so on.", global::Reqnroll.ProgrammingLanguage.CSharp, featureTags);
        
#line 1 "MatchPoints.feature"
#line hidden
        
        public virtual Microsoft.VisualStudio.TestTools.UnitTesting.TestContext TestContext
        {
            get
            {
                return this._testContext;
            }
            set
            {
                this._testContext = value;
            }
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static async global::System.Threading.Tasks.Task FeatureSetupAsync(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupAttribute(Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupBehavior.EndOfClass)]
        public static async global::System.Threading.Tasks.Task FeatureTearDownAsync()
        {
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestInitializeAttribute()]
        public async global::System.Threading.Tasks.Task TestInitializeAsync()
        {
            testRunner = global::Reqnroll.TestRunnerManager.GetTestRunnerForAssembly(featureHint: featureInfo);
            try
            {
                if (((testRunner.FeatureContext != null) 
                            && (testRunner.FeatureContext.FeatureInfo.Equals(featureInfo) == false)))
                {
                    await testRunner.OnFeatureEndAsync();
                }
            }
            finally
            {
                if (((testRunner.FeatureContext != null) 
                            && testRunner.FeatureContext.BeforeFeatureHookFailed))
                {
                    throw new global::Reqnroll.ReqnrollException("Scenario skipped because of previous before feature hook error");
                }
                if ((testRunner.FeatureContext == null))
                {
                    await testRunner.OnFeatureStartAsync(featureInfo);
                }
            }
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCleanupAttribute()]
        public async global::System.Threading.Tasks.Task TestTearDownAsync()
        {
            if ((testRunner == null))
            {
                return;
            }
            try
            {
                await testRunner.OnScenarioEndAsync();
            }
            finally
            {
                global::Reqnroll.TestRunnerManager.ReleaseTestRunner(testRunner);
                testRunner = null;
            }
        }
        
        public void ScenarioInitialize(global::Reqnroll.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Microsoft.VisualStudio.TestTools.UnitTesting.TestContext>(_testContext);
        }
        
        public async global::System.Threading.Tasks.Task ScenarioStartAsync()
        {
            await testRunner.OnScenarioStartAsync();
        }
        
        public async global::System.Threading.Tasks.Task ScenarioCleanupAsync()
        {
            await testRunner.CollectScenarioErrorsAsync();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Team A Wins Two Points")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "MatchPoints")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("tag1")]
        public async global::System.Threading.Tasks.Task TeamAWinsTwoPoints()
        {
            string[] tagsOfScenario = new string[] {
                    "tag1"};
            global::System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new global::System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("Team A Wins Two Points", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 7
  this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((global::Reqnroll.TagHelper.ContainsIgnoreTag(scenarioInfo.CombinedTags) || global::Reqnroll.TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
#line 8
    await testRunner.GivenAsync("Team A has 0 points won", ((string)(null)), ((global::Reqnroll.Table)(null)), "Given ");
#line hidden
#line 9
    await testRunner.AndAsync("Team B has 0 points won", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 10
    await testRunner.WhenAsync("Team A wins the next 2 points", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 11
    await testRunner.ThenAsync("The point count for Team A should be 30", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 12
    await testRunner.AndAsync("The point count for Team B should be 0", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Both Go To 40 Points")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "MatchPoints")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("tag1")]
        public async global::System.Threading.Tasks.Task BothGoTo40Points()
        {
            string[] tagsOfScenario = new string[] {
                    "tag1"};
            global::System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new global::System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("Both Go To 40 Points", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 15
  this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((global::Reqnroll.TagHelper.ContainsIgnoreTag(scenarioInfo.CombinedTags) || global::Reqnroll.TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
#line 16
    await testRunner.GivenAsync("Team A has 0 points won", ((string)(null)), ((global::Reqnroll.Table)(null)), "Given ");
#line hidden
#line 17
    await testRunner.AndAsync("Team B has 0 points won", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 18
    await testRunner.WhenAsync("Team A wins the next 3 points", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 19
    await testRunner.AndAsync("Team B wins the next 3 points", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 20
    await testRunner.ThenAsync("The point count for Team A should be 40", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 21
    await testRunner.AndAsync("The point count for Team B should be 40", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Both Have 40 Points, And a Team Scores")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "MatchPoints")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("tag1")]
        public async global::System.Threading.Tasks.Task BothHave40PointsAndATeamScores()
        {
            string[] tagsOfScenario = new string[] {
                    "tag1"};
            global::System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new global::System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("Both Have 40 Points, And a Team Scores", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 24
  this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((global::Reqnroll.TagHelper.ContainsIgnoreTag(scenarioInfo.CombinedTags) || global::Reqnroll.TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
#line 25
    await testRunner.GivenAsync("Team A has 3 points won", ((string)(null)), ((global::Reqnroll.Table)(null)), "Given ");
#line hidden
#line 26
    await testRunner.AndAsync("Team B has 3 points won", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 27
    await testRunner.WhenAsync("Team A wins the next 1 points", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 28
    await testRunner.ThenAsync("The point count for Team A should be Advantage", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 29
    await testRunner.AndAsync("The point count for Team B should be 40", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Move to new game")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "MatchPoints")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("tag1")]
        public async global::System.Threading.Tasks.Task MoveToNewGame()
        {
            string[] tagsOfScenario = new string[] {
                    "tag1"};
            global::System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new global::System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("Move to new game", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 32
  this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((global::Reqnroll.TagHelper.ContainsIgnoreTag(scenarioInfo.CombinedTags) || global::Reqnroll.TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
#line 33
    await testRunner.GivenAsync("Team A has 3 points won", ((string)(null)), ((global::Reqnroll.Table)(null)), "Given ");
#line hidden
#line 34
    await testRunner.AndAsync("Team B has 3 points won", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 35
    await testRunner.WhenAsync("Team A wins the next 2 points", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 36
    await testRunner.ThenAsync("the match should move to a new game", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 37
    await testRunner.AndAsync("the set count for Team A should be 1", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 38
    await testRunner.AndAsync("the set count for Team B should be 0", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Move to a new set")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "MatchPoints")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("tag1")]
        public async global::System.Threading.Tasks.Task MoveToANewSet()
        {
            string[] tagsOfScenario = new string[] {
                    "tag1"};
            global::System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new global::System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("Move to a new set", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 41
  this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((global::Reqnroll.TagHelper.ContainsIgnoreTag(scenarioInfo.CombinedTags) || global::Reqnroll.TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
#line 42
    await testRunner.GivenAsync("Team A has 0 set won", ((string)(null)), ((global::Reqnroll.Table)(null)), "Given ");
#line hidden
#line 43
    await testRunner.AndAsync("Team B has 0 set won", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 44
    await testRunner.AndAsync("the current set score is 5-4 in favor of Team A", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 45
    await testRunner.WhenAsync("Team A wins the next 4 points", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 46
    await testRunner.ThenAsync("the match should move to the next set", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 47
    await testRunner.AndAsync("the current set score should be reset to 0-0", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 48
    await testRunner.AndAsync("the won set count for Team A should be 1", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 49
    await testRunner.AndAsync("the won set count for Team B should be 0", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
    }
}
#pragma warning restore
#endregion
