using System.Collections.Generic;

public class CommerceSystemPosts
{
	public Dictionary<int, CommerceSystemPost> postTable;

	public CommerceSystemPosts()
    {
        postTable = new Dictionary<int, CommerceSystemPost>();
    }


    public static CommerceSystemPosts GetDefaultPosts()
    {
        return new CommerceSystemPosts()
        {
            postTable = new Dictionary<int, CommerceSystemPost>()
            {
                {1, new CommerceSystemPost() { id = 1 } },
                {2, new CommerceSystemPost() { id = 2 } },
                {3, new CommerceSystemPost() { id = 3 } },
                {4, new CommerceSystemPost() { id = 4 } },
                {5, new CommerceSystemPost() { id = 5 } },
                {6, new CommerceSystemPost() { id = 6 } },
                {7, new CommerceSystemPost() { id = 7 } },
                {8, new CommerceSystemPost() { id = 8 } },
                {9, new CommerceSystemPost() { id = 9 } },
            }
        };
    }
}
