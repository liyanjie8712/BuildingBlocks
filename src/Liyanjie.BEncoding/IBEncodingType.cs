using System.IO;

namespace Liyanjie.BEncoding
{
    public interface IBEncodingType
    {
        void Encode(BinaryWriter writer);
    }
}
