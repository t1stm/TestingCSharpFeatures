This class is made to support copying to multiple streams, without adding any delay from the Stream.CopyTo() method.

The reason I started writing this is because FFmpeg has some delay on writing to the Standard Input, thus causing the open HTTPS stream to timeout when the ending of the file is near.
