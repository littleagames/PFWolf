using LittleAGames.PFWolf.SDK.Components;

namespace LittleAGames.PFWolf.SDK.Tests.Components;

public class FaderTests
{
    
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    // TODO: Add test cases (1, 0)
    // 0, 1
    // 0.25, 0.50
    // .66, .33
    // 0, 0.33
    public void Fader_OnUpdate_Should_Calculate_CurrentOpacity()
    {
        // Arrange
        var fader = Fader.Create(1.0f, 0.0f, 0x00, 0x00, 0x00, 240);

        // Act
        fader.OnStart();
        fader.BeginFade();
        fader.OnUpdate();
        
        // Assert
        fader.CurrentOpacity.Should().Be(239.0f / 240.0f);
        fader.IsFading.Should().BeTrue();
        fader.IsComplete.Should().BeFalse();
    }
    
    [Test]
    public void Fader_FadeOut_OnUpdate_Should_Calculate_CurrentOpacity2()
    {
        // Arrange
        var fader = Fader.Create(0.0f, 1.0f, 0x00, 0x00, 0x00, 240);

        // Act
        fader.OnStart();
        fader.BeginFade();
        fader.OnUpdate();
        
        // Assert
        fader.CurrentOpacity.Should().Be(1.0f / 240.0f);
        fader.IsFading.Should().BeTrue();
        fader.IsComplete.Should().BeFalse();
    }
    
    [Test]
    public void Fader_Several_OnUpdate_Should_Calculate_CurrentOpacity()
    {
        // Arrange
        const short duration = 240;
        var fader = Fader.Create(1.0f, 0.0f, 0x00, 0x00, 0x00, duration);

        // Act
        fader.OnStart();
        fader.BeginFade();
        
        for (int i = 0; i < duration/2; i++)
            fader.OnUpdate();
        
        // Assert
        fader.CurrentOpacity.Should().Be(0.5f);
        fader.IsFading.Should().BeTrue();
        fader.IsComplete.Should().BeFalse();
    }
    
    [Test]
    public void Fader_When_DurationTimerHasNotStarted_Should_NotBeFading()
    {
        // Arrange
        const short duration = 240;
        var fader = Fader.Create(1.0f, 0.0f, 0x00, 0x00, 0x00, duration);

        // Act
        fader.OnStart();
        fader.BeginFade();
        
        // Assert
        fader.CurrentOpacity.Should().Be(1.0f);
        fader.IsFading.Should().BeFalse();
        fader.IsComplete.Should().BeFalse();
    }
    
    [Test]
    public void Fader_When_UpdatesHaveDepletedTime_Should_BeCompleteAndNotFading()
    {
        // Arrange
        const short duration = 240;
        var fader = Fader.Create(1.0f, 0.0f, 0x00, 0x00, 0x00, duration);

        // Act
        fader.OnStart();
        fader.BeginFade();
        
        for (int i = 0; i < duration; i++)
            fader.OnUpdate();
        
        // Assert
        fader.CurrentOpacity.Should().Be(0.0f);
        fader.IsFading.Should().BeFalse();
        fader.IsComplete.Should().BeTrue();
    }
}