using System.Collections.Generic;

public class CommerceSystemProducts
{
	public Dictionary<int, CommerceSystemProduct> productTable;

	public CommerceSystemProducts()
    {
        productTable = new Dictionary<int, CommerceSystemProduct>();
    }

    public static Dictionary<int, CommerceSystemProduct> GetDefaultProducts()
    {
        return new Dictionary<int, CommerceSystemProduct>
            {
                { 705, new CommerceSystemProduct()
                {
                    id = 705, price = 162, count = 1100,
                    stockTable = new Dictionary<int, COStockInfo>()
                    {
                        { 1, new COStockInfo() { idPost = 1, currentStock = 78, reserveStock = 0, price = 177 } },
                        { 2, new COStockInfo() { idPost = 2, currentStock = 78, reserveStock = 0, price = 177 } },
                        { 3, new COStockInfo() { idPost = 3, currentStock = 78, reserveStock = 0, price = 177 } },
                        { 4, new COStockInfo() { idPost = 4, currentStock = 78, reserveStock = 0, price = 177 } },
                        { 5, new COStockInfo() { idPost = 5, currentStock = 78, reserveStock = 0, price = 177 } },
                        { 6, new COStockInfo() { idPost = 6, currentStock = 78, reserveStock = 0, price = 177 } },
                        { 8, new COStockInfo() { idPost = 8, currentStock = 78, reserveStock = 0, price = 177 } },
                    }
                }
                },
                {704, new CommerceSystemProduct()
                {
                    id = 704, price = 1200, count = 54400,
                    stockTable = new Dictionary<int, COStockInfo>()
                    {
                        { 1, new COStockInfo() { idPost = 1, currentStock = 7771, reserveStock = 0, price = 1179 } },
                        { 2, new COStockInfo() { idPost = 2, currentStock = 7771, reserveStock = 0, price = 1179 } },
                        { 3, new COStockInfo() { idPost = 3, currentStock = 7771, reserveStock = 0, price = 1179 } },
                        { 4, new COStockInfo() { idPost = 4, currentStock = 7771, reserveStock = 0, price = 1179 } },
                        { 5, new COStockInfo() { idPost = 5, currentStock = 7771, reserveStock = 0, price = 1179 } },
                        { 6, new COStockInfo() { idPost = 6, currentStock = 7771, reserveStock = 0, price = 1179 } },
                        { 8, new COStockInfo() { idPost = 8, currentStock = 7771, reserveStock = 0, price = 1179 } },
                    }
                } },
                {703, new CommerceSystemProduct()
                {
                    id = 703, price = 200, count = 38800,
                    stockTable = new Dictionary<int, COStockInfo>()
                    {
                        { 1, new COStockInfo() { idPost = 1, currentStock = 5542, reserveStock = 0, price = 194 } },
                        { 2, new COStockInfo() { idPost = 2, currentStock = 5542, reserveStock = 0, price = 194 } },
                        { 3, new COStockInfo() { idPost = 3, currentStock = 5542, reserveStock = 0, price = 194 } },
                        { 4, new COStockInfo() { idPost = 4, currentStock = 5542, reserveStock = 0, price = 194 } },
                        { 5, new COStockInfo() { idPost = 5, currentStock = 5542, reserveStock = 0, price = 194 } },
                        { 6, new COStockInfo() { idPost = 6, currentStock = 5542, reserveStock = 0, price = 194 } },
                        { 8, new COStockInfo() { idPost = 8, currentStock = 5542, reserveStock = 0, price = 194 } },
                    }
                } },
                {702, new CommerceSystemProduct()
                {
                    id = 702, price = 60, count = 96000,
                    stockTable = new Dictionary<int, COStockInfo>()
                    {
                        { 1, new COStockInfo() { idPost = 1, currentStock = 13714, reserveStock = 0, price = 58 } },
                        { 2, new COStockInfo() { idPost = 2, currentStock = 13714, reserveStock = 0, price = 58 } },
                        { 3, new COStockInfo() { idPost = 3, currentStock = 13714, reserveStock = 0, price = 58 } },
                        { 4, new COStockInfo() { idPost = 4, currentStock = 13714, reserveStock = 0, price = 58 } },
                        { 5, new COStockInfo() { idPost = 5, currentStock = 13714, reserveStock = 0, price = 58 } },
                        { 6, new COStockInfo() { idPost = 6, currentStock = 13714, reserveStock = 0, price = 58 } },
                        { 8, new COStockInfo() { idPost = 8, currentStock = 13714, reserveStock = 0, price = 58 } },
                    }
                } },
                { 701,new CommerceSystemProduct()
                {
                    id = 701, price = 12, count = 136000,
                    stockTable = new Dictionary<int, COStockInfo>()
                    {
                        { 1, new COStockInfo() { idPost = 1, currentStock = 19428, reserveStock = 0, price = 11 } },
                        { 2, new COStockInfo() { idPost = 2, currentStock = 19428, reserveStock = 0, price = 11 } },
                        { 3, new COStockInfo() { idPost = 3, currentStock = 19428, reserveStock = 0, price = 11 } },
                        { 4, new COStockInfo() { idPost = 4, currentStock = 19428, reserveStock = 0, price = 11 } },
                        { 5, new COStockInfo() { idPost = 5, currentStock = 19428, reserveStock = 0, price = 11 } },
                        { 6, new COStockInfo() { idPost = 6, currentStock = 19428, reserveStock = 0, price = 11 } },
                        { 8, new COStockInfo() { idPost = 8, currentStock = 19428, reserveStock = 0, price = 11 } },
                    }
                } },
                {605, new CommerceSystemProduct()
                {
                    id = 605, price = 6750, count = 100,
                    stockTable = new Dictionary<int, COStockInfo>()
                    {
                        { 1, new COStockInfo() { idPost = 1, currentStock = 7, reserveStock = 0, price = 7410 } },
                        { 2, new COStockInfo() { idPost = 2, currentStock = 7, reserveStock = 0, price = 7410 } },
                        { 3, new COStockInfo() { idPost = 3, currentStock = 7, reserveStock = 0, price = 7410 } },
                        { 4, new COStockInfo() { idPost = 4, currentStock = 7, reserveStock = 0, price = 7410 } },
                        { 5, new COStockInfo() { idPost = 5, currentStock = 7, reserveStock = 0, price = 7410 } },
                        { 7, new COStockInfo() { idPost = 7, currentStock = 7, reserveStock = 0, price = 7410 } },
                        { 8, new COStockInfo() { idPost = 8, currentStock = 7, reserveStock = 0, price = 7410 } },
                    }
                } },
                {604, new CommerceSystemProduct()
                {
                    id = 604, price = 11000, count = 5440,
                    stockTable = new Dictionary<int, COStockInfo>()
                    {
                        { 1, new COStockInfo() { idPost = 1, currentStock = 777, reserveStock = 0, price = 10811 } },
                        { 2, new COStockInfo() { idPost = 2, currentStock = 777, reserveStock = 0, price = 10811 } },
                        { 3, new COStockInfo() { idPost = 3, currentStock = 777, reserveStock = 0, price = 10811 } },
                        { 4, new COStockInfo() { idPost = 4, currentStock = 777, reserveStock = 0, price = 10811 } },
                        { 5, new COStockInfo() { idPost = 5, currentStock = 777, reserveStock = 0, price = 10811 } },
                        { 7, new COStockInfo() { idPost = 7, currentStock = 777, reserveStock = 0, price = 10811 } },
                        { 8, new COStockInfo() { idPost = 8, currentStock = 777, reserveStock = 0, price = 10811 } },
                    }
                } },
                {603, new CommerceSystemProduct()
                {
                    id = 603, price = 2200, count = 6400,
                    stockTable = new Dictionary<int, COStockInfo>()
                    {
                        { 1, new COStockInfo() { idPost = 1, currentStock = 914, reserveStock = 0, price = 2143 } },
                        { 2, new COStockInfo() { idPost = 2, currentStock = 914, reserveStock = 0, price = 2143 } },
                        { 3, new COStockInfo() { idPost = 3, currentStock = 914, reserveStock = 0, price = 2143 } },
                        { 4, new COStockInfo() { idPost = 4, currentStock = 914, reserveStock = 0, price = 2143 } },
                        { 5, new COStockInfo() { idPost = 5, currentStock = 914, reserveStock = 0, price = 2143 } },
                        { 7, new COStockInfo() { idPost = 7, currentStock = 914, reserveStock = 0, price = 2143 } },
                        { 8, new COStockInfo() { idPost = 8, currentStock = 914, reserveStock = 0, price = 2143 } },
                    }
                } },
                {602, new CommerceSystemProduct()
                {
                    id = 602, price = 600, count = 9600,
                    stockTable = new Dictionary<int, COStockInfo>()
                    {
                        { 1, new COStockInfo() { idPost = 1, currentStock = 1371, reserveStock = 0, price = 579 } },
                        { 2, new COStockInfo() { idPost = 2, currentStock = 1371, reserveStock = 0, price = 579 } },
                        { 3, new COStockInfo() { idPost = 3, currentStock = 1371, reserveStock = 0, price = 579 } },
                        { 4, new COStockInfo() { idPost = 4, currentStock = 1371, reserveStock = 0, price = 579 } },
                        { 5, new COStockInfo() { idPost = 5, currentStock = 1371, reserveStock = 0, price = 579 } },
                        { 7, new COStockInfo() { idPost = 7, currentStock = 1371, reserveStock = 0, price = 579 } },
                        { 8, new COStockInfo() { idPost = 8, currentStock = 1371, reserveStock = 0, price = 579 } },
                    }
                } },
                {601, new CommerceSystemProduct()
                {
                    id = 601, price = 85, count = 13600,
                    stockTable = new Dictionary<int, COStockInfo>()
                    {
                        { 1, new COStockInfo() { idPost = 1, currentStock = 1942, reserveStock = 0, price = 79 } },
                        { 2, new COStockInfo() { idPost = 2, currentStock = 1942, reserveStock = 0, price = 79 } },
                        { 3, new COStockInfo() { idPost = 3, currentStock = 1942, reserveStock = 0, price = 79 } },
                        { 4, new COStockInfo() { idPost = 4, currentStock = 1942, reserveStock = 0, price = 79 } },
                        { 5, new COStockInfo() { idPost = 5, currentStock = 1942, reserveStock = 0, price = 79 } },
                        { 7, new COStockInfo() { idPost = 7, currentStock = 1942, reserveStock = 0, price = 79 } },
                        { 8, new COStockInfo() { idPost = 8, currentStock = 1942, reserveStock = 0, price = 79 } },
                    }
                } },
                {105, new CommerceSystemProduct()
                {
                    id = 105, price = 180, count = 1000,
                    stockTable = new Dictionary<int, COStockInfo>()
                    {
                        { 6, new COStockInfo() { idPost = 6, currentStock = 71, reserveStock = 0, price = 197 } },
                        { 2, new COStockInfo() { idPost = 2, currentStock = 71, reserveStock = 0, price = 197 } },
                        { 3, new COStockInfo() { idPost = 3, currentStock = 71, reserveStock = 0, price = 197 } },
                        { 4, new COStockInfo() { idPost = 4, currentStock = 71, reserveStock = 0, price = 197 } },
                        { 5, new COStockInfo() { idPost = 5, currentStock = 71, reserveStock = 0, price = 197 } },
                        { 7, new COStockInfo() { idPost = 7, currentStock = 71, reserveStock = 0, price = 197 } },
                        { 8, new COStockInfo() { idPost = 8, currentStock = 71, reserveStock = 0, price = 197 } },
                    }
                } },
                {803, new CommerceSystemProduct()
                {
                    id = 803, price = 13501, count = 800,
                    stockTable = new Dictionary<int, COStockInfo>()
                    {
                        { 1, new COStockInfo() { idPost = 1, currentStock = 114, reserveStock = 0, price = 13160 } },
                        { 2, new COStockInfo() { idPost = 2, currentStock = 114, reserveStock = 0, price = 13160 } },
                        { 3, new COStockInfo() { idPost = 3, currentStock = 114, reserveStock = 0, price = 13160 } },
                        { 4, new COStockInfo() { idPost = 4, currentStock = 114, reserveStock = 0, price = 13160 } },
                        { 5, new COStockInfo() { idPost = 5, currentStock = 114, reserveStock = 0, price = 13160 } },
                        { 6, new COStockInfo() { idPost = 6, currentStock = 114, reserveStock = 0, price = 13160 } },
                        { 7, new COStockInfo() { idPost = 7, currentStock = 114, reserveStock = 0, price = 13160 } },
                    }
                } },
                {505, new CommerceSystemProduct()
                {
                    id = 505, price = 540, count = 330,
                    stockTable = new Dictionary<int, COStockInfo>()
                    {
                        { 1, new COStockInfo() { idPost = 1, currentStock = 23, reserveStock = 0, price = 593 } },
                        { 2, new COStockInfo() { idPost = 2, currentStock = 23, reserveStock = 0, price = 593 } },
                        { 3, new COStockInfo() { idPost = 3, currentStock = 23, reserveStock = 0, price = 593 } },
                        { 4, new COStockInfo() { idPost = 4, currentStock = 23, reserveStock = 0, price = 593 } },
                        { 8, new COStockInfo() { idPost = 8, currentStock = 23, reserveStock = 0, price = 593 } },
                        { 6, new COStockInfo() { idPost = 6, currentStock = 23, reserveStock = 0, price = 593 } },
                        { 7, new COStockInfo() { idPost = 7, currentStock = 23, reserveStock = 0, price = 593 } },
                    }
                } },
                {504, new CommerceSystemProduct()
                {
                    id = 504, price = 450, count = 68000,
                    stockTable = new Dictionary<int, COStockInfo>()
                    {
                        { 1, new COStockInfo() { idPost = 1, currentStock = 9714, reserveStock = 0, price = 442 } },
                        { 2, new COStockInfo() { idPost = 2, currentStock = 9714, reserveStock = 0, price = 442 } },
                        { 3, new COStockInfo() { idPost = 3, currentStock = 9714, reserveStock = 0, price = 442 } },
                        { 4, new COStockInfo() { idPost = 4, currentStock = 9714, reserveStock = 0, price = 442 } },
                        { 8, new COStockInfo() { idPost = 8, currentStock = 9714, reserveStock = 0, price = 442 } },
                        { 6, new COStockInfo() { idPost = 6, currentStock = 9714, reserveStock = 0, price = 442 } },
                        { 7, new COStockInfo() { idPost = 7, currentStock = 9714, reserveStock = 0, price = 442 } },
                    }
                } },
                {503, new CommerceSystemProduct()
                {
                    id = 503, price = 140, count = 128000,
                    stockTable = new Dictionary<int, COStockInfo>()
                    {
                        { 1, new COStockInfo() { idPost = 1, currentStock = 18285, reserveStock = 0, price = 136 } },
                        { 2, new COStockInfo() { idPost = 2, currentStock = 18285, reserveStock = 0, price = 136 } },
                        { 3, new COStockInfo() { idPost = 3, currentStock = 18285, reserveStock = 0, price = 136 } },
                        { 4, new COStockInfo() { idPost = 4, currentStock = 18285, reserveStock = 0, price = 136 } },
                        { 8, new COStockInfo() { idPost = 8, currentStock = 18285, reserveStock = 0, price = 136 } },
                        { 6, new COStockInfo() { idPost = 6, currentStock = 18285, reserveStock = 0, price = 136 } },
                        { 7, new COStockInfo() { idPost = 7, currentStock = 18285, reserveStock = 0, price = 136 } },
                    }
                } },
                {502, new CommerceSystemProduct()
                {
                    id = 502, price = 40, count = 68000,
                    stockTable = new Dictionary<int, COStockInfo>()
                    {
                        { 1, new COStockInfo() { idPost = 1, currentStock = 9714, reserveStock = 0, price = 39 } },
                        { 2, new COStockInfo() { idPost = 2, currentStock = 9714, reserveStock = 0, price = 39 } },
                        { 3, new COStockInfo() { idPost = 3, currentStock = 9714, reserveStock = 0, price = 39 } },
                        { 4, new COStockInfo() { idPost = 4, currentStock = 9714, reserveStock = 0, price = 39 } },
                        { 8, new COStockInfo() { idPost = 8, currentStock = 9714, reserveStock = 0, price = 39 } },
                        { 6, new COStockInfo() { idPost = 6, currentStock = 9714, reserveStock = 0, price = 39 } },
                        { 7, new COStockInfo() { idPost = 7, currentStock = 9714, reserveStock = 0, price = 39 } },
                    }
                } },
                {501, new CommerceSystemProduct()
                {
                    id = 501, price = 8, count = 272000,
                    stockTable = new Dictionary<int, COStockInfo>()
                    {
                        { 1, new COStockInfo() { idPost = 1, currentStock = 2764, reserveStock = 0, price = 11 } },
                        { 2, new COStockInfo() { idPost = 2, currentStock = 2764, reserveStock = 0, price = 11 } },
                        { 3, new COStockInfo() { idPost = 3, currentStock = 2764, reserveStock = 0, price = 11 } },
                        { 4, new COStockInfo() { idPost = 4, currentStock = 2764, reserveStock = 0, price = 11 } },
                        { 8, new COStockInfo() { idPost = 8, currentStock = 2764, reserveStock = 0, price = 11 } },
                        { 6, new COStockInfo() { idPost = 6, currentStock = 2764, reserveStock = 0, price = 11 } },
                        { 7, new COStockInfo() { idPost = 7, currentStock = 2764, reserveStock = 0, price = 11 } },
                    }
                } },
                {103, new CommerceSystemProduct()
                {
                    id = 103, price = 100, count = 128000,
                    stockTable = new Dictionary<int, COStockInfo>()
                    {
                        { 1, new COStockInfo() { idPost = 5, currentStock = 18285, reserveStock = 0, price = 97 } },
                        { 2, new COStockInfo() { idPost = 2, currentStock = 18285, reserveStock = 0, price = 97 } },
                        { 3, new COStockInfo() { idPost = 3, currentStock = 18285, reserveStock = 0, price = 97 } },
                        { 4, new COStockInfo() { idPost = 4, currentStock = 18285, reserveStock = 0, price = 97 } },
                        { 8, new COStockInfo() { idPost = 8, currentStock = 18285, reserveStock = 0, price = 97 } },
                        { 6, new COStockInfo() { idPost = 6, currentStock = 18285, reserveStock = 0, price = 97 } },
                        { 7, new COStockInfo() { idPost = 7, currentStock = 18285, reserveStock = 0, price = 97 } },
                    }
                } },
                {405, new CommerceSystemProduct()
                {
                    id = 405, price = 360, count = 500,
                    stockTable = new Dictionary<int, COStockInfo>()
                    {
                        { 1, new COStockInfo() { idPost = 1, currentStock = 35, reserveStock = 0, price = 395 } },
                        { 2, new COStockInfo() { idPost = 2, currentStock = 35, reserveStock = 0, price = 395 } },
                        { 3, new COStockInfo() { idPost = 3, currentStock = 35, reserveStock = 0, price = 395 } },
                        { 5, new COStockInfo() { idPost = 5, currentStock = 35, reserveStock = 0, price = 395 } },
                        { 6, new COStockInfo() { idPost = 6, currentStock = 35, reserveStock = 0, price = 395 } },
                        { 7, new COStockInfo() { idPost = 7, currentStock = 35, reserveStock = 0, price = 395 } },
                        { 8, new COStockInfo() { idPost = 8, currentStock = 35, reserveStock = 0, price = 395 } },
                    }
                } },
                {404, new CommerceSystemProduct()
                {
                    id = 404, price = 800, count = 54400,
                    stockTable = new Dictionary<int, COStockInfo>()
                    {
                        { 1, new COStockInfo() { idPost = 1, currentStock = 7771, reserveStock = 0, price = 786 } },
                        { 2, new COStockInfo() { idPost = 2, currentStock = 7771, reserveStock = 0, price = 786 } },
                        { 3, new COStockInfo() { idPost = 3, currentStock = 7771, reserveStock = 0, price = 786 } },
                        { 5, new COStockInfo() { idPost = 5, currentStock = 7771, reserveStock = 0, price = 786 } },
                        { 6, new COStockInfo() { idPost = 6, currentStock = 7771, reserveStock = 0, price = 786 } },
                        { 7, new COStockInfo() { idPost = 7, currentStock = 7771, reserveStock = 0, price = 786 } },
                        { 8, new COStockInfo() { idPost = 8, currentStock = 7771, reserveStock = 0, price = 786 } },
                    }
                } },
                {403, new CommerceSystemProduct()
                {
                    id = 403, price = 200, count = 54400,
                    stockTable = new Dictionary<int, COStockInfo>()
                    {
                        { 1, new COStockInfo() { idPost = 1, currentStock = 7771, reserveStock = 0, price = 194 } },
                        { 2, new COStockInfo() { idPost = 2, currentStock = 7771, reserveStock = 0, price = 194 } },
                        { 3, new COStockInfo() { idPost = 3, currentStock = 7771, reserveStock = 0, price = 194 } },
                        { 5, new COStockInfo() { idPost = 5, currentStock = 7771, reserveStock = 0, price = 194 } },
                        { 6, new COStockInfo() { idPost = 6, currentStock = 7771, reserveStock = 0, price = 194 } },
                        { 7, new COStockInfo() { idPost = 7, currentStock = 7771, reserveStock = 0, price = 194 } },
                        { 8, new COStockInfo() { idPost = 8, currentStock = 7771, reserveStock = 0, price = 194 } },
                    }
                } },
                {402, new CommerceSystemProduct()
                {
                    id = 402, price = 55, count = 96000,
                    stockTable = new Dictionary<int, COStockInfo>()
                    {
                        { 1, new COStockInfo() { idPost = 1, currentStock = 13714, reserveStock = 0, price = 53 } },
                        { 2, new COStockInfo() { idPost = 2, currentStock = 13714, reserveStock = 0, price = 53 } },
                        { 3, new COStockInfo() { idPost = 3, currentStock = 13714, reserveStock = 0, price = 53 } },
                        { 5, new COStockInfo() { idPost = 5, currentStock = 13714, reserveStock = 0, price = 53 } },
                        { 6, new COStockInfo() { idPost = 6, currentStock = 13714, reserveStock = 0, price = 53 } },
                        { 7, new COStockInfo() { idPost = 7, currentStock = 13714, reserveStock = 0, price = 53 } },
                        { 8, new COStockInfo() { idPost = 8, currentStock = 13714, reserveStock = 0, price = 53 } },
                    }
                } },
                {401, new CommerceSystemProduct()
                {
                    id = 401, price = 12, count = 136000,
                    stockTable = new Dictionary<int, COStockInfo>()
                    {
                        { 1, new COStockInfo() { idPost = 1, currentStock = 19428, reserveStock = 0, price = 11 } },
                        { 2, new COStockInfo() { idPost = 2, currentStock = 19428, reserveStock = 0, price = 11 } },
                        { 3, new COStockInfo() { idPost = 3, currentStock = 19428, reserveStock = 0, price = 11 } },
                        { 5, new COStockInfo() { idPost = 5, currentStock = 19428, reserveStock = 0, price = 11 } },
                        { 6, new COStockInfo() { idPost = 6, currentStock = 19428, reserveStock = 0, price = 11 } },
                        { 7, new COStockInfo() { idPost = 7, currentStock = 19428, reserveStock = 0, price = 11 } },
                        { 8, new COStockInfo() { idPost = 8, currentStock = 19428, reserveStock = 0, price = 11 } },
                    }
                } },
                {305, new CommerceSystemProduct()
                {
                    id = 305, price = 450, count = 400,
                    stockTable = new Dictionary<int, COStockInfo>()
                    {
                        { 1, new COStockInfo() { idPost = 1, currentStock = 28, reserveStock = 0, price = 494 } },
                        { 2, new COStockInfo() { idPost = 2, currentStock = 28, reserveStock = 0, price = 494 } },
                        { 4, new COStockInfo() { idPost = 4, currentStock = 28, reserveStock = 0, price = 494 } },
                        { 5, new COStockInfo() { idPost = 5, currentStock = 28, reserveStock = 0, price = 494 } },
                        { 6, new COStockInfo() { idPost = 6, currentStock = 28, reserveStock = 0, price = 494 } },
                        { 7, new COStockInfo() { idPost = 7, currentStock = 28, reserveStock = 0, price = 494 } },
                        { 8, new COStockInfo() { idPost = 8, currentStock = 28, reserveStock = 0, price = 494 } },
                    }
                } },
                {304, new CommerceSystemProduct()
                {
                    id = 304, price = 4500, count = 11824,
                    stockTable = new Dictionary<int, COStockInfo>()
                    {
                        { 1, new COStockInfo() { idPost = 1, currentStock = 1689, reserveStock = 0, price = 4422 } },
                        { 2, new COStockInfo() { idPost = 2, currentStock = 1689, reserveStock = 0, price = 4422 } },
                        { 4, new COStockInfo() { idPost = 4, currentStock = 1689, reserveStock = 0, price = 4422 } },
                        { 5, new COStockInfo() { idPost = 5, currentStock = 1689, reserveStock = 0, price = 4422 } },
                        { 6, new COStockInfo() { idPost = 6, currentStock = 1689, reserveStock = 0, price = 4422 } },
                        { 7, new COStockInfo() { idPost = 7, currentStock = 1689, reserveStock = 0, price = 4422 } },
                        { 8, new COStockInfo() { idPost = 8, currentStock = 1689, reserveStock = 0, price = 4422 } },
                    }
                } },
                {303, new CommerceSystemProduct()
                {
                    id = 303, price = 1500, count = 12800,
                    stockTable = new Dictionary<int, COStockInfo>()
                    {
                        { 1, new COStockInfo() { idPost = 1, currentStock = 1828, reserveStock = 0, price = 1461 } },
                        { 2, new COStockInfo() { idPost = 2, currentStock = 1828, reserveStock = 0, price = 1461 } },
                        { 4, new COStockInfo() { idPost = 4, currentStock = 1828, reserveStock = 0, price = 1461 } },
                        { 5, new COStockInfo() { idPost = 5, currentStock = 1828, reserveStock = 0, price = 1461 } },
                        { 6, new COStockInfo() { idPost = 6, currentStock = 1828, reserveStock = 0, price = 1461 } },
                        { 7, new COStockInfo() { idPost = 7, currentStock = 1828, reserveStock = 0, price = 1461 } },
                        { 8, new COStockInfo() { idPost = 8, currentStock = 1828, reserveStock = 0, price = 1461 } },
                    }
                } },
                {302, new CommerceSystemProduct()
                {
                    id = 302, price = 350, count = 19200,
                    stockTable = new Dictionary<int, COStockInfo>()
                    {
                        { 1, new COStockInfo() { idPost = 1, currentStock = 2742, reserveStock = 0, price = 338 } },
                        { 2, new COStockInfo() { idPost = 2, currentStock = 2742, reserveStock = 0, price = 338 } },
                        { 4, new COStockInfo() { idPost = 4, currentStock = 2742, reserveStock = 0, price = 338 } },
                        { 5, new COStockInfo() { idPost = 5, currentStock = 2742, reserveStock = 0, price = 338 } },
                        { 6, new COStockInfo() { idPost = 6, currentStock = 2742, reserveStock = 0, price = 338 } },
                        { 7, new COStockInfo() { idPost = 7, currentStock = 2742, reserveStock = 0, price = 338 } },
                        { 8, new COStockInfo() { idPost = 8, currentStock = 2742, reserveStock = 0, price = 338 } },
                    }
                } },
                {301, new CommerceSystemProduct()
                {
                    id = 301, price = 48, count = 27200,
                    stockTable = new Dictionary<int, COStockInfo>()
                    {
                        { 1, new COStockInfo() { idPost = 1, currentStock = 3885, reserveStock = 0, price = 44 } },
                        { 2, new COStockInfo() { idPost = 2, currentStock = 3885, reserveStock = 0, price = 44 } },
                        { 4, new COStockInfo() { idPost = 4, currentStock = 3885, reserveStock = 0, price = 44 } },
                        { 5, new COStockInfo() { idPost = 5, currentStock = 3885, reserveStock = 0, price = 44 } },
                        { 6, new COStockInfo() { idPost = 6, currentStock = 3885, reserveStock = 0, price = 44 } },
                        { 7, new COStockInfo() { idPost = 7, currentStock = 3885, reserveStock = 0, price = 44 } },
                        { 8, new COStockInfo() { idPost = 8, currentStock = 3885, reserveStock = 0, price = 44 } },
                    }
                } },
                {205, new CommerceSystemProduct()
                {
                    id = 205, price = 900, count = 200,
                    stockTable = new Dictionary<int, COStockInfo>()
                    {
                        { 1, new COStockInfo() { idPost = 1, currentStock = 14, reserveStock = 0, price = 988 } },
                        { 3, new COStockInfo() { idPost = 3, currentStock = 14, reserveStock = 0, price = 988 } },
                        { 4, new COStockInfo() { idPost = 4, currentStock = 14, reserveStock = 0, price = 988 } },
                        { 5, new COStockInfo() { idPost = 5, currentStock = 14, reserveStock = 0, price = 988 } },
                        { 6, new COStockInfo() { idPost = 6, currentStock = 14, reserveStock = 0, price = 988 } },
                        { 7, new COStockInfo() { idPost = 7, currentStock = 14, reserveStock = 0, price = 988 } },
                        { 8, new COStockInfo() { idPost = 8, currentStock = 14, reserveStock = 0, price = 988 } },
                    }
                } },
                {204, new CommerceSystemProduct()
                {
                    id = 204, price = 5500, count = 12400,
                    stockTable = new Dictionary<int, COStockInfo>()
                    {
                        { 1, new COStockInfo() { idPost = 1, currentStock = 1771, reserveStock = 0, price = 5405 } },
                        { 3, new COStockInfo() { idPost = 3, currentStock = 1771, reserveStock = 0, price = 5405 } },
                        { 4, new COStockInfo() { idPost = 4, currentStock = 1771, reserveStock = 0, price = 5405 } },
                        { 5, new COStockInfo() { idPost = 5, currentStock = 1771, reserveStock = 0, price = 5405 } },
                        { 6, new COStockInfo() { idPost = 6, currentStock = 1771, reserveStock = 0, price = 5405 } },
                        { 7, new COStockInfo() { idPost = 7, currentStock = 1771, reserveStock = 0, price = 5405 } },
                        { 8, new COStockInfo() { idPost = 8, currentStock = 1771, reserveStock = 0, price = 5405 } },
                    }
                } },
                {203, new CommerceSystemProduct()
                {
                    id = 203, price = 251, count = 64000,
                    stockTable = new Dictionary<int, COStockInfo>()
                    {
                        { 1, new COStockInfo() { idPost = 1, currentStock = 9142, reserveStock = 0, price = 244 } },
                        { 3, new COStockInfo() { idPost = 3, currentStock = 9142, reserveStock = 0, price = 244 } },
                        { 4, new COStockInfo() { idPost = 4, currentStock = 9142, reserveStock = 0, price = 244 } },
                        { 5, new COStockInfo() { idPost = 5, currentStock = 9142, reserveStock = 0, price = 244 } },
                        { 6, new COStockInfo() { idPost = 6, currentStock = 9142, reserveStock = 0, price = 244 } },
                        { 7, new COStockInfo() { idPost = 7, currentStock = 9142, reserveStock = 0, price = 244 } },
                        { 8, new COStockInfo() { idPost = 8, currentStock = 9142, reserveStock = 0, price = 244 } },
                    }
                } },
                {202, new CommerceSystemProduct()
                {
                    id = 202, price = 180, count = 19200,
                    stockTable = new Dictionary<int, COStockInfo>()
                    {
                        { 1, new COStockInfo() { idPost = 1, currentStock = 2742, reserveStock = 0, price = 174 } },
                        { 3, new COStockInfo() { idPost = 3, currentStock = 2742, reserveStock = 0, price = 174 } },
                        { 4, new COStockInfo() { idPost = 4, currentStock = 2742, reserveStock = 0, price = 174 } },
                        { 5, new COStockInfo() { idPost = 5, currentStock = 2742, reserveStock = 0, price = 174 } },
                        { 6, new COStockInfo() { idPost = 6, currentStock = 2742, reserveStock = 0, price = 174 } },
                        { 7, new COStockInfo() { idPost = 7, currentStock = 2742, reserveStock = 0, price = 174 } },
                        { 8, new COStockInfo() { idPost = 8, currentStock = 2742, reserveStock = 0, price = 174 } },
                    }
                } },
                {201, new CommerceSystemProduct()
                {
                    id = 201, price = 40, count = 27200,
                    stockTable = new Dictionary<int, COStockInfo>()
                    {
                        { 2, new COStockInfo() { idPost = 2, currentStock = 3885, reserveStock = 0, price = 37 } },
                        { 3, new COStockInfo() { idPost = 3, currentStock = 3885, reserveStock = 0, price = 37 } },
                        { 4, new COStockInfo() { idPost = 4, currentStock = 3885, reserveStock = 0, price = 37 } },
                        { 5, new COStockInfo() { idPost = 5, currentStock = 3885, reserveStock = 0, price = 37 } },
                        { 6, new COStockInfo() { idPost = 6, currentStock = 3885, reserveStock = 0, price = 37 } },
                        { 7, new COStockInfo() { idPost = 7, currentStock = 3885, reserveStock = 0, price = 37 } },
                        { 8, new COStockInfo() { idPost = 8, currentStock = 3885, reserveStock = 0, price = 37 } },
                    }
                } },
                {908, new CommerceSystemProduct()
                {
                    id = 908, price = 17407, count = 250,
                    stockTable = new Dictionary<int, COStockInfo>()
                    {
                        { 1, new COStockInfo() { idPost = 1, currentStock = 31, reserveStock = 0, price = 17423 } },
                        { 2, new COStockInfo() { idPost = 2, currentStock = 31, reserveStock = 0, price = 17423 } },
                        { 3, new COStockInfo() { idPost = 3, currentStock = 31, reserveStock = 0, price = 17423 } },
                        { 4, new COStockInfo() { idPost = 4, currentStock = 31, reserveStock = 0, price = 17423 } },
                        { 5, new COStockInfo() { idPost = 5, currentStock = 31, reserveStock = 0, price = 17423 } },
                        { 6, new COStockInfo() { idPost = 6, currentStock = 31, reserveStock = 0, price = 17423 } },
                        { 7, new COStockInfo() { idPost = 7, currentStock = 31, reserveStock = 0, price = 17423 } },
                        { 8, new COStockInfo() { idPost = 8, currentStock = 31, reserveStock = 0, price = 17423 } },
                    }
                } },
                {907, new CommerceSystemProduct()
                {
                    id = 907, price = 5047, count = 550,
                    stockTable = new Dictionary<int, COStockInfo>()
                    {
                        { 1, new COStockInfo() { idPost = 1, currentStock = 68, reserveStock = 0, price = 5049 } },
                        { 2, new COStockInfo() { idPost = 2, currentStock = 68, reserveStock = 0, price = 5049 } },
                        { 3, new COStockInfo() { idPost = 3, currentStock = 68, reserveStock = 0, price = 5049 } },
                        { 4, new COStockInfo() { idPost = 4, currentStock = 68, reserveStock = 0, price = 5049 } },
                        { 5, new COStockInfo() { idPost = 5, currentStock = 68, reserveStock = 0, price = 5049 } },
                        { 6, new COStockInfo() { idPost = 6, currentStock = 68, reserveStock = 0, price = 5049 } },
                        { 7, new COStockInfo() { idPost = 7, currentStock = 68, reserveStock = 0, price = 5049 } },
                        { 8, new COStockInfo() { idPost = 8, currentStock = 68, reserveStock = 0, price = 5049 } },
                    }
                } },
                {906, new CommerceSystemProduct()
                {
                    id = 906, price = 8343, count = 38800,
                    stockTable = new Dictionary<int, COStockInfo>()
                    {
                        { 1, new COStockInfo() { idPost = 1, currentStock = 4850, reserveStock = 0, price = 8343 } },
                        { 2, new COStockInfo() { idPost = 2, currentStock = 4850, reserveStock = 0, price = 8343 } },
                        { 3, new COStockInfo() { idPost = 3, currentStock = 4850, reserveStock = 0, price = 8343 } },
                        { 4, new COStockInfo() { idPost = 4, currentStock = 4850, reserveStock = 0, price = 8343 } },
                        { 5, new COStockInfo() { idPost = 5, currentStock = 4850, reserveStock = 0, price = 8343 } },
                        { 6, new COStockInfo() { idPost = 6, currentStock = 4850, reserveStock = 0, price = 8343 } },
                        { 7, new COStockInfo() { idPost = 7, currentStock = 4850, reserveStock = 0, price = 8343 } },
                        { 8, new COStockInfo() { idPost = 8, currentStock = 4850, reserveStock = 0, price = 8343 } },
                    }
                } },
                {905, new CommerceSystemProduct()
                {
                    id = 905, price = 6592, count = 54400,
                    stockTable = new Dictionary<int, COStockInfo>()
                    {
                        { 1, new COStockInfo() { idPost = 1, currentStock = 6800, reserveStock = 0, price = 6592 } },
                        { 2, new COStockInfo() { idPost = 2, currentStock = 6800, reserveStock = 0, price = 6592 } },
                        { 3, new COStockInfo() { idPost = 3, currentStock = 6800, reserveStock = 0, price = 6592 } },
                        { 4, new COStockInfo() { idPost = 4, currentStock = 6800, reserveStock = 0, price = 6592 } },
                        { 5, new COStockInfo() { idPost = 5, currentStock = 6800, reserveStock = 0, price = 6592 } },
                        { 6, new COStockInfo() { idPost = 6, currentStock = 6800, reserveStock = 0, price = 6592 } },
                        { 7, new COStockInfo() { idPost = 7, currentStock = 6800, reserveStock = 0, price = 6592 } },
                        { 8, new COStockInfo() { idPost = 8, currentStock = 6800, reserveStock = 0, price = 6592 } },
                    }
                } },
                {904, new CommerceSystemProduct()
                {
                    id = 904, price = 5047, count = 38800,
                    stockTable = new Dictionary<int, COStockInfo>()
                    {
                        { 1, new COStockInfo() { idPost = 1, currentStock = 4850, reserveStock = 0, price = 5047 } },
                        { 2, new COStockInfo() { idPost = 2, currentStock = 4850, reserveStock = 0, price = 5047 } },
                        { 3, new COStockInfo() { idPost = 3, currentStock = 4850, reserveStock = 0, price = 5047 } },
                        { 4, new COStockInfo() { idPost = 4, currentStock = 4850, reserveStock = 0, price = 5047 } },
                        { 5, new COStockInfo() { idPost = 5, currentStock = 4850, reserveStock = 0, price = 5047 } },
                        { 6, new COStockInfo() { idPost = 6, currentStock = 4850, reserveStock = 0, price = 5047 } },
                        { 7, new COStockInfo() { idPost = 7, currentStock = 4850, reserveStock = 0, price = 5047 } },
                        { 8, new COStockInfo() { idPost = 8, currentStock = 4850, reserveStock = 0, price = 5047 } },
                    }
                } },
                {903, new CommerceSystemProduct()
                {
                    id = 903, price = 2575, count = 54400,
                    stockTable = new Dictionary<int, COStockInfo>()
                    {
                        { 1, new COStockInfo() { idPost = 1, currentStock = 6800, reserveStock = 0, price = 2575 } },
                        { 2, new COStockInfo() { idPost = 2, currentStock = 6800, reserveStock = 0, price = 2575 } },
                        { 3, new COStockInfo() { idPost = 3, currentStock = 6800, reserveStock = 0, price = 2575 } },
                        { 4, new COStockInfo() { idPost = 4, currentStock = 6800, reserveStock = 0, price = 2575 } },
                        { 5, new COStockInfo() { idPost = 5, currentStock = 6800, reserveStock = 0, price = 2575 } },
                        { 6, new COStockInfo() { idPost = 6, currentStock = 6800, reserveStock = 0, price = 2575 } },
                        { 7, new COStockInfo() { idPost = 7, currentStock = 6800, reserveStock = 0, price = 2575 } },
                        { 8, new COStockInfo() { idPost = 8, currentStock = 6800, reserveStock = 0, price = 2575 } },
                    }
                } },
                {902, new CommerceSystemProduct()
                {
                    id = 902, price = 3708, count = 68000,
                    stockTable = new Dictionary<int, COStockInfo>()
                    {
                        { 1, new COStockInfo() { idPost = 1, currentStock = 8500, reserveStock = 0, price = 3708 } },
                        { 2, new COStockInfo() { idPost = 2, currentStock = 8500, reserveStock = 0, price = 3708 } },
                        { 3, new COStockInfo() { idPost = 3, currentStock = 8500, reserveStock = 0, price = 3708 } },
                        { 4, new COStockInfo() { idPost = 4, currentStock = 8500, reserveStock = 0, price = 3708 } },
                        { 5, new COStockInfo() { idPost = 5, currentStock = 8500, reserveStock = 0, price = 3708 } },
                        { 6, new COStockInfo() { idPost = 6, currentStock = 8500, reserveStock = 0, price = 3708 } },
                        { 7, new COStockInfo() { idPost = 7, currentStock = 8500, reserveStock = 0, price = 3708 } },
                        { 8, new COStockInfo() { idPost = 8, currentStock = 8500, reserveStock = 0, price = 3708 } },
                    }
                } },
                {901, new CommerceSystemProduct()
                {
                    id = 901, price = 1648, count = 136000,
                    stockTable = new Dictionary<int, COStockInfo>()
                    {
                        { 1, new COStockInfo() { idPost = 1, currentStock = 17000, reserveStock = 0, price = 1648 } },
                        { 2, new COStockInfo() { idPost = 2, currentStock = 17000, reserveStock = 0, price = 1648 } },
                        { 3, new COStockInfo() { idPost = 3, currentStock = 17000, reserveStock = 0, price = 1648 } },
                        { 4, new COStockInfo() { idPost = 4, currentStock = 17000, reserveStock = 0, price = 1648 } },
                        { 5, new COStockInfo() { idPost = 5, currentStock = 17000, reserveStock = 0, price = 1648 } },
                        { 6, new COStockInfo() { idPost = 6, currentStock = 17000, reserveStock = 0, price = 1648 } },
                        { 7, new COStockInfo() { idPost = 7, currentStock = 17000, reserveStock = 0, price = 1648 } },
                        { 8, new COStockInfo() { idPost = 8, currentStock = 17000, reserveStock = 0, price = 1648 } },
                    }
                } },
                {101, new CommerceSystemProduct()
                {
                    id = 101, price = 8, count = 272000,
                    stockTable = new Dictionary<int, COStockInfo>()
                    {
                        { 2, new COStockInfo() { idPost = 2, currentStock = 2764, reserveStock = 0, price = 11 } },
                        { 3, new COStockInfo() { idPost = 3, currentStock = 2764, reserveStock = 0, price = 11 } },
                        { 4, new COStockInfo() { idPost = 4, currentStock = 2764, reserveStock = 0, price = 11 } },
                        { 5, new COStockInfo() { idPost = 5, currentStock = 2764, reserveStock = 0, price = 11 } },
                        { 6, new COStockInfo() { idPost = 6, currentStock = 2764, reserveStock = 0, price = 11 } },
                        { 7, new COStockInfo() { idPost = 7, currentStock = 2764, reserveStock = 0, price = 11 } },
                        { 8, new COStockInfo() { idPost = 8, currentStock = 2764, reserveStock = 0, price = 11 } },
                    }
                } },
                {104, new CommerceSystemProduct()
                {
                    id = 104, price = 400, count = 90664,
                    stockTable = new Dictionary<int, COStockInfo>()
                    {
                        { 2, new COStockInfo() { idPost = 2, currentStock = 12952, reserveStock = 0, price = 393 } },
                        { 3, new COStockInfo() { idPost = 3, currentStock = 12952, reserveStock = 0, price = 393 } },
                        { 4, new COStockInfo() { idPost = 4, currentStock = 12952, reserveStock = 0, price = 393 } },
                        { 5, new COStockInfo() { idPost = 5, currentStock = 12952, reserveStock = 0, price = 393 } },
                        { 6, new COStockInfo() { idPost = 6, currentStock = 12952, reserveStock = 0, price = 393 } },
                        { 7, new COStockInfo() { idPost = 7, currentStock = 12952, reserveStock = 0, price = 393 } },
                        { 8, new COStockInfo() { idPost = 8, currentStock = 12952, reserveStock = 0, price = 393 } },
                    }
                } },
                {805, new CommerceSystemProduct()
                {
                    id = 805, price = 900, count = 200,
                    stockTable = new Dictionary<int, COStockInfo>()
                    {
                        { 1, new COStockInfo() { idPost = 1, currentStock = 14, reserveStock = 0, price = 988 } },
                        { 2, new COStockInfo() { idPost = 2, currentStock = 14, reserveStock = 0, price = 988 } },
                        { 3, new COStockInfo() { idPost = 3, currentStock = 14, reserveStock = 0, price = 988 } },
                        { 4, new COStockInfo() { idPost = 4, currentStock = 14, reserveStock = 0, price = 988 } },
                        { 5, new COStockInfo() { idPost = 5, currentStock = 14, reserveStock = 0, price = 988 } },
                        { 6, new COStockInfo() { idPost = 6, currentStock = 14, reserveStock = 0, price = 988 } },
                        { 7, new COStockInfo() { idPost = 7, currentStock = 14, reserveStock = 0, price = 988 } },
                    }
                } },
                {804, new CommerceSystemProduct()
                {
                    id = 804, price = 28000, count = 1280,
                    stockTable = new Dictionary<int, COStockInfo>()
                    {
                        { 1, new COStockInfo() { idPost = 1, currentStock = 182, reserveStock = 0, price = 27538 } },
                        { 2, new COStockInfo() { idPost = 2, currentStock = 182, reserveStock = 0, price = 27538 } },
                        { 3, new COStockInfo() { idPost = 3, currentStock = 182, reserveStock = 0, price = 27538 } },
                        { 4, new COStockInfo() { idPost = 4, currentStock = 182, reserveStock = 0, price = 27538 } },
                        { 5, new COStockInfo() { idPost = 5, currentStock = 182, reserveStock = 0, price = 27538 } },
                        { 6, new COStockInfo() { idPost = 6, currentStock = 182, reserveStock = 0, price = 27538 } },
                        { 7, new COStockInfo() { idPost = 7, currentStock = 182, reserveStock = 0, price = 27538 } },
                    }
                } },
                {802, new CommerceSystemProduct()
                {
                    id = 802, price = 200, count = 13560,
                    stockTable = new Dictionary<int, COStockInfo>()
                    {
                        { 1, new COStockInfo() { idPost = 1, currentStock = 1937, reserveStock = 0, price = 193 } },
                        { 2, new COStockInfo() { idPost = 2, currentStock = 1937, reserveStock = 0, price = 193 } },
                        { 3, new COStockInfo() { idPost = 3, currentStock = 1937, reserveStock = 0, price = 193 } },
                        { 4, new COStockInfo() { idPost = 4, currentStock = 1937, reserveStock = 0, price = 193 } },
                        { 5, new COStockInfo() { idPost = 5, currentStock = 1937, reserveStock = 0, price = 193 } },
                        { 6, new COStockInfo() { idPost = 6, currentStock = 1937, reserveStock = 0, price = 193 } },
                        { 7, new COStockInfo() { idPost = 7, currentStock = 1937, reserveStock = 0, price = 193 } },
                    }
                } },
                {801, new CommerceSystemProduct()
                {
                    id = 801, price = 40, count = 27200,
                    stockTable = new Dictionary<int, COStockInfo>()
                    {
                        { 1, new COStockInfo() { idPost = 1, currentStock = 3885, reserveStock = 0, price = 37 } },
                        { 2, new COStockInfo() { idPost = 2, currentStock = 3885, reserveStock = 0, price = 37 } },
                        { 3, new COStockInfo() { idPost = 3, currentStock = 3885, reserveStock = 0, price = 37 } },
                        { 4, new COStockInfo() { idPost = 4, currentStock = 3885, reserveStock = 0, price = 37 } },
                        { 5, new COStockInfo() { idPost = 5, currentStock = 3885, reserveStock = 0, price = 37 } },
                        { 6, new COStockInfo() { idPost = 6, currentStock = 3885, reserveStock = 0, price = 37 } },
                        { 7, new COStockInfo() { idPost = 7, currentStock = 3885, reserveStock = 0, price = 37 } },
                    }
                } },
            };
    }
}
