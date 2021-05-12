using Caduhd.Common;
using Caduhd.Controller.InputAnalyzer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Caduhd.Controller.Tests.InputAnalyzer
{
    public class HandsAnalyzerTests
    {
        private HandsAnalyzer _handsAnalyzer;
        private BgrImage _left;
        private List<Point> _leftPoi; 
        private BgrImage _right;
        private List<Point> _rightPoi; 

        public HandsAnalyzerTests()
        {
            _handsAnalyzer = new HandsAnalyzer();
            _left = BgrImage.GetBlank(640, 480, Color.Red);
            _leftPoi = new List<Point>();
            _right = BgrImage.GetBlank(640, 480, Color.Green);
            _rightPoi = new List<Point>();
        }
        
        [Fact]
        public void AnalyzeLeft_NotInAnalyzingLeftHandState_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => _handsAnalyzer.AnalyzeLeft(_left, _leftPoi));
        }

        [Fact]
        public void AnalyzeRight_NotInAnalyzingRightHandState_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => _handsAnalyzer.AnalyzeLeft(_right, _rightPoi));
        }

        [Fact]
        public void AdvanceState_ReadyToAnalyzeLeftState_AdvancesToAnalyzingLeftState()
        {
            Assert.Equal(HandsAnalyzerState.ReadyToAnalyzeLeft, _handsAnalyzer.State);
            _handsAnalyzer.AdvanceState();
            Assert.Equal(HandsAnalyzerState.AnalyzingLeft, _handsAnalyzer.State);
        }

        [Fact]
        public void AdvanceState_AnalyzingLeftState_LeftNotAnalyzed_ThrowsInvalidOperationException()
        {
            _handsAnalyzer.AdvanceState();
            Assert.Equal(HandsAnalyzerState.AnalyzingLeft, _handsAnalyzer.State);
            Assert.Throws<InvalidOperationException>(() => _handsAnalyzer.AdvanceState());
        }

        [Fact]
        public void AdvanceState_AnalyzingLeftState_LeftAnalyzed_AdvancesToReadyToAnalyreRightState()
        {
            _handsAnalyzer.AdvanceState();
            Assert.Equal(HandsAnalyzerState.AnalyzingLeft, _handsAnalyzer.State);

            _handsAnalyzer.AnalyzeLeft(_left, _leftPoi);

            _handsAnalyzer.AdvanceState();
            Assert.Equal(HandsAnalyzerState.ReadyToAnalyzeRight, _handsAnalyzer.State);
        }

        [Fact]
        public void AdvanceState_ReadyToAnalyzeRightState_AdvancesToAnalyzingRightState()
        {
            _handsAnalyzer.AdvanceState();
            _handsAnalyzer.AnalyzeLeft(_left, _leftPoi);
            _handsAnalyzer.AdvanceState();

            _handsAnalyzer.AdvanceState();
            Assert.Equal(HandsAnalyzerState.AnalyzingRight, _handsAnalyzer.State);
        }

        [Fact]
        public void AdvanceState_AnalyzingRightState_RightNotAnalyzed_ThrowsInvalidOperationException()
        {
            _handsAnalyzer.AdvanceState();
            _handsAnalyzer.AnalyzeLeft(_left, _leftPoi);
            _handsAnalyzer.AdvanceState();

            _handsAnalyzer.AdvanceState();
            Assert.Throws<InvalidOperationException>(() => _handsAnalyzer.AdvanceState());
        }

        [Fact]
        public void AdvanceState_AnalyzingRightState_RightAnalyzed_AdvancesToTuningState()
        {
            _handsAnalyzer.AdvanceState();
            _handsAnalyzer.AnalyzeLeft(_left, _leftPoi);
            _handsAnalyzer.AdvanceState();

            _handsAnalyzer.AdvanceState();
            _handsAnalyzer.AnalyzeRight(_right, _rightPoi);
            _handsAnalyzer.AdvanceState();

            Assert.Equal(HandsAnalyzerState.Tuning, _handsAnalyzer.State);
        }

        [Fact]
        public void AdvanceState_SwitchesBetweenHandsAnalyzerStatesCorrectly()
        {
            Assert.Equal(HandsAnalyzerState.ReadyToAnalyzeLeft, _handsAnalyzer.State);
            _handsAnalyzer.AdvanceState();
            Assert.Equal(HandsAnalyzerState.AnalyzingLeft, _handsAnalyzer.State);
            _handsAnalyzer.AnalyzeLeft(_left, _leftPoi);
            _handsAnalyzer.AdvanceState();
            Assert.Equal(HandsAnalyzerState.ReadyToAnalyzeRight, _handsAnalyzer.State);
            _handsAnalyzer.AdvanceState();
            Assert.Equal(HandsAnalyzerState.AnalyzingRight, _handsAnalyzer.State);
            _handsAnalyzer.AnalyzeRight(_right, _rightPoi);
            _handsAnalyzer.AdvanceState();
            Assert.Equal(HandsAnalyzerState.Tuning, _handsAnalyzer.State);
            _handsAnalyzer.AdvanceState();
            Assert.Equal(HandsAnalyzerState.ReadyToAnalyzeLeft, _handsAnalyzer.State);
        }

        [Fact]
        public void ResultGetter_NoHandsHaveBeenAnalyzed_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => _handsAnalyzer.Result);
        }

        [Fact]
        public void ResultGetter_OnlyLeftAnalyzed_ThrowsInvalidOperationException()
        {
            _handsAnalyzer.AdvanceState();
            _handsAnalyzer.AnalyzeLeft(_left, _leftPoi);

            Assert.Throws<InvalidOperationException>(() => _handsAnalyzer.Result);
        }

        [Fact]
        public void ResultGetter_BothHandsAnalyzed_ReturnsEvaluatedResult()
        {
            _handsAnalyzer.AdvanceState();
            _handsAnalyzer.AnalyzeLeft(_left, _leftPoi);
            _handsAnalyzer.AdvanceState();

            _handsAnalyzer.AdvanceState();
            _handsAnalyzer.AnalyzeRight(_right, _rightPoi);

            Assert.NotNull(_handsAnalyzer.Result);
        }

        [Fact]
        public void ResultGetter_BothHandsAnalyzed_ReturnsCorrectResult()
        {
            _handsAnalyzer.AdvanceState();
            _handsAnalyzer.AnalyzeLeft(_left, _leftPoi);
            _handsAnalyzer.AdvanceState();

            _handsAnalyzer.AdvanceState();
            _handsAnalyzer.AnalyzeRight(_right, _rightPoi);

            var handsAnalyzerResult = _handsAnalyzer.Result;

            Assert.True(CompareImagesPixelByPixel(_left.Merge(_right), handsAnalyzerResult.HandsForeground));
            Assert.True(CompareImagesPixelByPixel(_right.Merge(_left), handsAnalyzerResult.HandsBackground));
        }

        [Fact]
        public void Reset_StateIsNotReadyToAnalyzeLeft_SetsStateBackToReadyToAnalyzeLeft()
        {
            _handsAnalyzer.AdvanceState();
            Assert.NotEqual(HandsAnalyzerState.ReadyToAnalyzeLeft, _handsAnalyzer.State);
        }

        private bool CompareImagesPixelByPixel(BgrImage a, BgrImage b)
        {
            if (a.Width != b.Width || a.Height != b.Height)
                return false;

            for (int y = 0; y < a.Height; y++)
                for (int x = 0; x < b.Width; x++)
                    if (!AreBgrPixelsTheSame(a.GetPixel(x, y), b.GetPixel(x, y)))
                        return false;

            return true;
        }

        private bool AreBgrPixelsTheSame(BgrPixel a, BgrPixel b) =>
            a.Blue == b.Blue &&
            a.Green == b.Green &&
            a.Red == b.Red;
    }
}
