namespace Battleships
{
    public class Board
    {
        private BoardFieldStatus[,] board;

        public void AddFlag((int x, int y) zeroBasedCoordinates, BoardFieldStatus alreadyFiredUpon)
        {
            board[zeroBasedCoordinates.x, zeroBasedCoordinates.y] |= alreadyFiredUpon;
        }

        public BoardFieldStatus GetFieldStatus((int x, int y) zeroBasedCoordinates)
        {
            return board[zeroBasedCoordinates.x, zeroBasedCoordinates.y];
        }

        public void SetFlag((int x, int y) zeroBasedCoordinates, BoardFieldStatus alreadyFiredUpon)
        {
            board[zeroBasedCoordinates.x, zeroBasedCoordinates.y] = alreadyFiredUpon;
        }

        public void SetupBoard(GameOptions gameOptions)
        {
            board = new BoardFieldStatus[gameOptions.BoardWidth, gameOptions.BoardHeight];
            for (int x = 0; x <= board.GetUpperBound(0); x++)
            {
                for (int y = 0; y <= board.GetUpperBound(1); y++)
                {
                    board[x, y] = BoardFieldStatus.Empty;
                }
            }
        }
    }
}