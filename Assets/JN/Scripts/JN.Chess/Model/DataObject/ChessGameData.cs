using System.Collections.Generic;
using UnityEngine;

namespace JN.Chess
{
    public class ChessGameData
    {
		static Dictionary<char, PieceType> DictPieceTypeSymbol = new Dictionary<char, PieceType>()
		{
			['k'] = PieceType.King,
			['p'] = PieceType.Pawn,
			['n'] = PieceType.Knight,
			['b'] = PieceType.Bishop,
			['r'] = PieceType.Rook,
			['q'] = PieceType.Queen
		};

		public const string START_FEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

        public int[] squares;
		public List<PieceData> pieces;
        public bool whiteCastleKingside;
        public bool whiteCastleQueenside;
        public bool blackCastleKingside;
        public bool blackCastleQueenside;
        public int epFile;
        public TeamColor ActiveColor;
        public int plyCount;

        public ChessGameData()
        {
            squares = new int[64];
        }

        public void LoadFromFEN(string FEN)
        {
			pieces = new List<PieceData>();

            string[] section = FEN.Split(' ');

			LoadPiecePosition(section[0]);
			LoadActiveColor(section[1]);
			// LoadCastling(section[2]);
			// LoadEnPassant(section[3]);
			// LoadHalfMove(section[4]);
			// LoadFullMofe(section[5]);
        }

        private void LoadPiecePosition(string FEN)
        {
			Debug.Log("juju " + FEN);

			int column = 0;
			int row = 7;

			foreach (char symbol in FEN) 
			{
				if (symbol == '/') 
				{
					column = 0;
					row--;
				} 
				else 
				{
					if (char.IsDigit (symbol)) 
					{
						column += (int) char.GetNumericValue (symbol);
					} 
					else 
					{
						PieceData pData = new PieceData();
						pData.teamColor = (char.IsUpper (symbol)) ? TeamColor.White : TeamColor.Black;
						pData.pieceType = DictPieceTypeSymbol[char.ToLower(symbol)];
						pData.position = new Vector2Int(column, row);
						pieces.Add(pData);
						
						column++;
					}
				}
			}
        }

        public void LoadActiveColor(string FEN)
        {
			ActiveColor = (FEN == "w") ? TeamColor.White : TeamColor.Black;
        }

        // public static void LoadCastling(string FEN)
        // {
		// 	string castlingRights = (sections.Length > 2) ? sections[2] : "KQkq";
		// 	loadedPositionInfo.whiteCastleKingside = castlingRights.Contains ("K");
		// 	loadedPositionInfo.whiteCastleQueenside = castlingRights.Contains ("Q");
		// 	loadedPositionInfo.blackCastleKingside = castlingRights.Contains ("k");
		// 	loadedPositionInfo.blackCastleQueenside = castlingRights.Contains ("q");
        // }


        // private static void LoadEnPassant(string v)
        // {
        //     throw new NotImplementedException();
        // }

		// private static void LoadFullMofe(string v)
        // {
        //     throw new NotImplementedException();
        // }

        // private static void LoadHalfMove(string v)
        // {
        //     throw new NotImplementedException();
        // }
    }
}