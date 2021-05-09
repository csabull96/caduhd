using Caduhd.Common;
using Caduhd.Drone.Dji;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Drone.Tests.Dji
{
    public class TelloStateParserTests
    {
        private TelloStateParser _telloStateParser;

        public TelloStateParserTests()
        {
            _telloStateParser = new TelloStateParser();
        }

        [Fact]
        public void parse_()
        {
            var telloStateAsBytes = "pitch:27;roll:-29;yaw:4;vgx:0;vgy:0;vgz:0;templ:48;temph:50;tof:10;h:0;bat:92;baro:264.02;time:0;agx:470.00;agy:454.00;agz:-772.00;".AsBytes();
            var parsedState = _telloStateParser.Parse(telloStateAsBytes);

            Assert.Equal(27, parsedState.Pitch);
            Assert.Equal(-29, parsedState.Roll);
            Assert.Equal(4, parsedState.Yaw);
            Assert.Equal(0, parsedState.SpeedX);
            Assert.Equal(0, parsedState.SpeedY);
            Assert.Equal(0, parsedState.SpeedZ);
            Assert.Equal(48, parsedState.LowestTemperature);
            Assert.Equal(50, parsedState.HighestTemperature);
            Assert.Equal(10, parsedState.ToF);
            Assert.Equal(0, parsedState.Height);
            Assert.Equal(92, parsedState.Battery);
            Assert.Equal(264.02, parsedState.Barometer);
            Assert.Equal(0, parsedState.Time);
            Assert.Equal(470.00, parsedState.AccelerationX);
            Assert.Equal(454.00, parsedState.AccelerationY);
            Assert.Equal(-772.00, parsedState.AccelerationZ);
        }

        [Fact]
        public void Parse_Sample1_ParsedCorrectly()
        {
            var telloStateAsBytes = "pitch:27;roll:-29;yaw:4;vgx:0;vgy:0;vgz:0;templ:48;temph:50;tof:10;h:0;bat:92;baro:264.02;time:0;agx:470.00;agy:454.00;agz:-772.00;".AsBytes();
            var parsedState = _telloStateParser.Parse(telloStateAsBytes);

            Assert.Equal(27, parsedState.Pitch);
            Assert.Equal(-29, parsedState.Roll);
            Assert.Equal(4, parsedState.Yaw);
            Assert.Equal(0, parsedState.SpeedX);
            Assert.Equal(0, parsedState.SpeedY);
            Assert.Equal(0, parsedState.SpeedZ);
            Assert.Equal(48, parsedState.LowestTemperature);
            Assert.Equal(50, parsedState.HighestTemperature);
            Assert.Equal(10, parsedState.ToF);
            Assert.Equal(0, parsedState.Height);
            Assert.Equal(92, parsedState.Battery);
            Assert.Equal(264.02, parsedState.Barometer);
            Assert.Equal(0, parsedState.Time);
            Assert.Equal(470.00, parsedState.AccelerationX);
            Assert.Equal(454.00, parsedState.AccelerationY);
            Assert.Equal(-772.00, parsedState.AccelerationZ);
        }

        [Fact]
        public void Parse_Sample2_ParsedCorrectly()
        {
            var telloStateAsBytes = "pitch:1;roll:17;yaw:-148;vgx:0;vgy:0;vgz:0;templ:49;temph:51;tof:10;h:0;bat:91;baro:264.05;time:0;agx:37.00;agy:-361.00;agz:-982.00;".AsBytes();
            var parsedState = _telloStateParser.Parse(telloStateAsBytes);

            Assert.Equal(1, parsedState.Pitch);
            Assert.Equal(17, parsedState.Roll);
            Assert.Equal(-148, parsedState.Yaw);
            Assert.Equal(0, parsedState.SpeedX);
            Assert.Equal(0, parsedState.SpeedY);
            Assert.Equal(0, parsedState.SpeedZ);
            Assert.Equal(49, parsedState.LowestTemperature);
            Assert.Equal(51, parsedState.HighestTemperature);
            Assert.Equal(10, parsedState.ToF);
            Assert.Equal(0, parsedState.Height);
            Assert.Equal(91, parsedState.Battery);
            Assert.Equal(264.05, parsedState.Barometer);
            Assert.Equal(0, parsedState.Time);
            Assert.Equal(37.00, parsedState.AccelerationX);
            Assert.Equal(-361.00, parsedState.AccelerationY);
            Assert.Equal(-982.00, parsedState.AccelerationZ);
        }

    }
}
