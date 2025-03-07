using FluentAssertions;

namespace ChessLogic.Tests.Unit;
public class BoardTests
{
    private readonly Board _sut;
    public BoardTests()
    {
        _sut = new Board();
    }

    [Fact]
    public void IsInside_ShouldReturnTrue_WhenCasillaIsInsideBoard()
    {
        // Arrange
        int fila = Random.Shared.Next(0, 7);
        int col = Random.Shared.Next(0, 7);
        var square = new Square(fila, col);

        // Act
        var result = Board.IsInside(square);

        // Asert
        result.Should().BeTrue();
    }


    [Fact]
    public void IsInside_ShouldReturnFalse_WhenCasillaIsOutsideBoard()
    {
        // Arrange
        int fila = Random.Shared.Next(8, 10);
        int col = Random.Shared.Next(8, 10);
        var square = new Square(fila, col);

        // Act
        var result = Board.IsInside(square);

        // Asert
        result.Should().BeFalse();
    }
}
