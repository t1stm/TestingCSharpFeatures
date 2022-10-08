using Terminal.Gui;

Application.Init();

var top = Application.Top;
var frame = top.Frame;

var window = new Window("ThirtyDollarApp") {
    X = 0,
    Y = 1,
    Width = Dim.Fill(),
    Height = Dim.Fill() - 1
};

top.Add(window);

Application.Run();