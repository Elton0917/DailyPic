<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DailyPic._default" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8">
    <title>DailyPicture</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="">
    <meta name="author" content="Bean">

    <!-- Le styles -->
    <link href="Content/bootstrap/bootstrap.css" rel="stylesheet" />
      <script src="Scripts/jquery-2.0.3.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <%-- Header --%>
    <div class="navbar navbar-inverse navbar-fixed-top">
      <div class="container">
        <div class="navbar-header">
          <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
          </button>
          <a class="navbar-brand" href="#">Daily Picture</a>
        </div>
        <div class="navbar-collapse collapse">
          <ul class="nav navbar-nav">
    <li class="active"><a href="Default.aspx">上傳</a></li>
            <li><a href="Calendar.aspx">日曆</a></li>
          </ul>
        </div><!--/.navbar-collapse -->
      </div>
    </div>
      <%-- Header end --%>
      <%-- Body end --%>
    <div class="jumbotron">
      <div class="container">
            <br />
        <div id="Image" runat="server">

            <%--<input type="button" class="btn btn-info" id="getpic" value="選擇檔案"/>
            <script>
                $("#getpic").click(function () {$("#file1").click() });
            </script>--%>
            <label for="file1">找相簿</label>
                        <input type="file" id="file1" name="file1" class="btn btn-success" accept="image/*" capture="camera" runat="server"/>
                        
             <br />
            <output id="list" for="file1" class="ClinicalImg"></output>
             <br />
            <input type="text" class="form-control" placeholder="標題" id="pictitle" runat="server"/>
            <textarea class="form-control" rows="3" placeholder="註記" id="piccontent" runat="server"></textarea>
            <br />
            <asp:Button ID="Button1" runat="server" Text="上傳" cssclass="btn btn-primary" OnClick="Button1_Click"/>
            </div>
            </div>
    </div>
        </form>
    <script src="Scripts/bootstrap.js"></script>
                        <script>
                            function handleFileSelect(evt) {
                                //loading icon
                                var files = evt.target.files; // FileList object
                                // Loop through the FileList and render image files as thumbnails.
                                for (var i = 0, f; f = files[i]; i++) {
                                    // Only process image files.
                                    if (!f.type.match('image.*')) {
                                        continue;
                                    }
                                    var reader = new FileReader();
                                    // Closure to capture the file information.
                                    reader.onload = (function (theFile) {
                                        return function (e) {
                                            // Render thumbnail.
                                            var span = document.createElement('span');
                                            span.innerHTML = ['<img class="img-responsive" src="', e.target.result,
                                                            '" title="', escape(theFile.name), '"/>'].join('');
                                            document.getElementById('list').insertBefore(span, null);
                                        };
                                    })(f);
                                    // Read in the image file as a data URL.
                                    reader.readAsDataURL(f);
                                }
                            } document.getElementById('file1').addEventListener('change', handleFileSelect, false);
                            </script>
            
</body>
</html>
