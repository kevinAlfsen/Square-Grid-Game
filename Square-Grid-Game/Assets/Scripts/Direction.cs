public enum Direction {
    N, NE, E, SE, S, SW, W, NW 
}

public static class DirectionExtensions {
    public static Direction Opposite (this Direction direction) {
        return (int) direction < 4 ? (direction + 4) : (direction - 4);
    }
}