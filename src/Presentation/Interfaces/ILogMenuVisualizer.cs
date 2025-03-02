namespace Presentation.Interfaces;

public interface ILogMenuVisualizer : ILogCalendarVisualizer, ILogChartVisualizer, ILogTableVisualizer
{
    void ShowVisualization();
}