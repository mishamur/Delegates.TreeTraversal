using System;
using System.Collections.Generic;
using System.Linq;

namespace Delegates.TreeTraversal
{
    public static class Traversal
    {
        public static IEnumerable<Product> GetProducts(ProductCategory root)
        {
            return TreeTraversals<ProductCategory>.Calculate(
                root,
                x => x.Categories,
                x => x.Products,
                x => true)
            .SelectMany(x => x);
        }

        public static IEnumerable<Job> GetEndJobs(Job root)
        {
            return TreeTraversals<Job>.Calculate(
                root,
                x => x.Subjobs,
                x => x,
                x => x.Subjobs.Count == 0
                );
        }

        public static IEnumerable<T> GetBinaryTreeValues<T>(BinaryTree<T> root)
        {
            return TreeTraversals<BinaryTree<T>>.Calculate(
                root,
                x => new BinaryTree<T>[] { x.Left, x.Right },
                x => x.Value,
                x => x.Right == null && x.Left == null
               );
        }
    }

    public class TreeTraversals<TypeData>
    {
        public static IEnumerable<ReturnType> Calculate<ReturnType>(
            TypeData root,
            Func<TypeData, IEnumerable<TypeData>> getChilds,
            Func<TypeData, ReturnType> returnValue,
            Func<TypeData, bool> predicate)
        {
            if (predicate.Invoke(root))
                yield return returnValue.Invoke(root);
            

            var chids = getChilds?.Invoke(root);
            foreach (var child in chids)
            {
                if(child != null)
                    foreach(var value in Calculate(child, getChilds, returnValue, predicate))
                        yield return value;
            }
        }
    }
}
