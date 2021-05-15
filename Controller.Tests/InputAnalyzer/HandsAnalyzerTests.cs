namespace Caduhd.Controller.Tests.InputAnalyzer
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using Caduhd.Common;
    using Caduhd.Controller.InputAnalyzer;
    using Xunit;

    public class HandsAnalyzerTests
    {
        private readonly HandsAnalyzer handsAnalyzer;
        private readonly BgrImage left;
        private readonly List<Point> leftPoi;
        private readonly BgrImage right;
        private readonly List<Point> rightPoi;

        public HandsAnalyzerTests()
        {
            this.handsAnalyzer = new HandsAnalyzer();
            this.left = BgrImage.GetBlank(640, 480, Color.Red);
            this.leftPoi = new List<Point>();
            this.right = BgrImage.GetBlank(640, 480, Color.Green);
            this.rightPoi = new List<Point>();
        }

        [Fact]
        public void AnalyzeLeft_NotInAnalyzingLeftHandState_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => this.handsAnalyzer.AnalyzeLeft(this.left, this.leftPoi));
        }

        [Fact]
        public void AnalyzeRight_NotInAnalyzingRightHandState_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => this.handsAnalyzer.AnalyzeLeft(this.right, this.rightPoi));
        }

        [Fact]
        public void AdvanceState_ReadyToAnalyzeLeftState_AdvancesToAnalyzingLeftState()
        {
            Assert.Equal(HandsAnalyzerState.ReadyToAnalyzeLeft, this.handsAnalyzer.State);
            this.handsAnalyzer.AdvanceState();
            Assert.Equal(HandsAnalyzerState.AnalyzingLeft, this.handsAnalyzer.State);
        }

        [Fact]
        public void AdvanceState_AnalyzingLeftState_LeftNotAnalyzed_ThrowsInvalidOperationException()
        {
            this.handsAnalyzer.AdvanceState();
            Assert.Equal(HandsAnalyzerState.AnalyzingLeft, this.handsAnalyzer.State);
            Assert.Throws<InvalidOperationException>(() => this.handsAnalyzer.AdvanceState());
        }

        [Fact]
        public void AdvanceState_AnalyzingLeftState_LeftAnalyzed_AdvancesToReadyToAnalyreRightState()
        {
            this.handsAnalyzer.AdvanceState();
            Assert.Equal(HandsAnalyzerState.AnalyzingLeft, this.handsAnalyzer.State);

            this.handsAnalyzer.AnalyzeLeft(this.left, this.leftPoi);

            this.handsAnalyzer.AdvanceState();
            Assert.Equal(HandsAnalyzerState.ReadyToAnalyzeRight, this.handsAnalyzer.State);
        }

        [Fact]
        public void AdvanceState_ReadyToAnalyzeRightState_AdvancesToAnalyzingRightState()
        {
            this.handsAnalyzer.AdvanceState();
            this.handsAnalyzer.AnalyzeLeft(this.left, this.leftPoi);
            this.handsAnalyzer.AdvanceState();

            this.handsAnalyzer.AdvanceState();
            Assert.Equal(HandsAnalyzerState.AnalyzingRight, this.handsAnalyzer.State);
        }

        [Fact]
        public void AdvanceState_AnalyzingRightState_RightNotAnalyzed_ThrowsInvalidOperationException()
        {
            this.handsAnalyzer.AdvanceState();
            this.handsAnalyzer.AnalyzeLeft(this.left, this.leftPoi);
            this.handsAnalyzer.AdvanceState();

            this.handsAnalyzer.AdvanceState();
            Assert.Throws<InvalidOperationException>(() => this.handsAnalyzer.AdvanceState());
        }

        [Fact]
        public void AdvanceState_AnalyzingRightState_RightAnalyzed_AdvancesToTuningState()
        {
            this.handsAnalyzer.AdvanceState();
            this.handsAnalyzer.AnalyzeLeft(this.left, this.leftPoi);
            this.handsAnalyzer.AdvanceState();

            this.handsAnalyzer.AdvanceState();
            this.handsAnalyzer.AnalyzeRight(this.right, this.rightPoi);
            this.handsAnalyzer.AdvanceState();

            Assert.Equal(HandsAnalyzerState.Tuning, this.handsAnalyzer.State);
        }

        [Fact]
        public void AdvanceState_SwitchesBetweenHandsAnalyzerStatesCorrectly()
        {
            Assert.Equal(HandsAnalyzerState.ReadyToAnalyzeLeft, this.handsAnalyzer.State);
            this.handsAnalyzer.AdvanceState();
            Assert.Equal(HandsAnalyzerState.AnalyzingLeft, this.handsAnalyzer.State);
            this.handsAnalyzer.AnalyzeLeft(this.left, this.leftPoi);
            this.handsAnalyzer.AdvanceState();
            Assert.Equal(HandsAnalyzerState.ReadyToAnalyzeRight, this.handsAnalyzer.State);
            this.handsAnalyzer.AdvanceState();
            Assert.Equal(HandsAnalyzerState.AnalyzingRight, this.handsAnalyzer.State);
            this.handsAnalyzer.AnalyzeRight(this.right, this.rightPoi);
            this.handsAnalyzer.AdvanceState();
            Assert.Equal(HandsAnalyzerState.Tuning, this.handsAnalyzer.State);
            this.handsAnalyzer.AdvanceState();
            Assert.Equal(HandsAnalyzerState.ReadyToAnalyzeLeft, this.handsAnalyzer.State);
        }

        [Fact]
        public void ResultGetter_NoHandsHaveBeenAnalyzed_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => this.handsAnalyzer.Result);
        }

        [Fact]
        public void ResultGetter_OnlyLeftAnalyzed_ThrowsInvalidOperationException()
        {
            this.handsAnalyzer.AdvanceState();
            this.handsAnalyzer.AnalyzeLeft(this.left, this.leftPoi);

            Assert.Throws<InvalidOperationException>(() => this.handsAnalyzer.Result);
        }

        [Fact]
        public void ResultGetter_BothHandsAnalyzed_ReturnsEvaluatedResult()
        {
            this.handsAnalyzer.AdvanceState();
            this.handsAnalyzer.AnalyzeLeft(this.left, this.leftPoi);
            this.handsAnalyzer.AdvanceState();

            this.handsAnalyzer.AdvanceState();
            this.handsAnalyzer.AnalyzeRight(this.right, this.rightPoi);

            Assert.NotNull(this.handsAnalyzer.Result);
        }

        [Fact]
        public void ResultGetter_BothHandsAnalyzed_ReturnsCorrectResult()
        {
            this.handsAnalyzer.AdvanceState();
            this.handsAnalyzer.AnalyzeLeft(this.left, this.leftPoi);
            this.handsAnalyzer.AdvanceState();

            this.handsAnalyzer.AdvanceState();
            this.handsAnalyzer.AnalyzeRight(this.right, this.rightPoi);

            var handsAnalyzerResult = this.handsAnalyzer.Result;

            Assert.True(this.CompareImagesPixelByPixel(this.left.Merge(this.right), handsAnalyzerResult.HandsForeground));
            Assert.True(this.CompareImagesPixelByPixel(this.right.Merge(this.left), handsAnalyzerResult.HandsBackground));
        }

        [Fact]
        public void Reset_StateIsNotReadyToAnalyzeLeft_SetsStateBackToReadyToAnalyzeLeft()
        {
            this.handsAnalyzer.AdvanceState();
            Assert.NotEqual(HandsAnalyzerState.ReadyToAnalyzeLeft, this.handsAnalyzer.State);
        }

        private bool CompareImagesPixelByPixel(BgrImage a, BgrImage b)
        {
            if (a.Width != b.Width || a.Height != b.Height)
            {
                return false;
            }

            for (int y = 0; y < a.Height; y++)
            {
                for (int x = 0; x < b.Width; x++)
                {
                    if (!this.AreBgrPixelsTheSame(a.GetPixel(x, y), b.GetPixel(x, y)))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool AreBgrPixelsTheSame(BgrPixel a, BgrPixel b) =>
            a.Blue == b.Blue &&
            a.Green == b.Green &&
            a.Red == b.Red;
    }
}
