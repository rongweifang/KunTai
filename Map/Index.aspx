<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="Index" %>

<!doctype html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="chrome=1" />
    <meta name="viewport" content="initial-scale=1.0, user-scalable=no, width=device-width" />
    <link href="CSS/Style.css" rel="stylesheet" />
    <script src="js/marker.js"></script>
    <script src="http://webapi.amap.com/maps?v=1.3&key=5cf9725bf4e81b6978ac9ee83165420f"></script>
    <script src="http://lib.sinaapp.com/js/jquery/1.9.1/jquery-1.9.1.min.js"></script>
    <script src="layer/layer.js"></script>
    <title>启明星宇</title>
</head>
<body>
    <div id="container" tabindex="0"></div>
    <script type="text/javascript">

        var mapObj = new AMap.Map('container', { resizeEnable: true, zoom: 5 });

        var markers = []; //province见Demo引用的JS文件
        for (var i = 0; i < provinces.length; i += 1) {
            var marker;

            marker = new AMap.Marker({
                position: provinces[i].center.split(','),
                title: provinces[i].name,
                map: mapObj
            });
            if (provinces[i].type === 1) {
                var content = "<p class=\"triangle-right\" style=\"width:80px;height:60px;top:-104px;left:-70px;\"><a onClick=\"ShowInfo('" + provinces[i].ID + "','" + provinces[i].name + "')\"><img src=\"" + provinces[i].img + "\" width=\"80px\" height=\"60px\"></a></p>";
                var baodao = new AMap.Marker({
                    content: content,

                    position: provinces[i].center.split(','),
                    title: provinces[i].name,
                    offset: new AMap.Pixel(0, 0),
                    map: mapObj
                });

            }

            markers.push(marker);
        }

        function ShowInfo(ID, CityName) {

            layer.open({
                type: 1,
                title: '' + CityName + '--2016年12月15日星期四',
                area: ['700px', '500px'],
                shadeClose: true, 
                content: '\<\div style="padding:20px;">' + GetInfo(ID) + '\<\/div>'
            });
        }

        function GetInfo(ID) {
            var sContent = "";
            switch (ID) {
                case "11":
                    sContent = GetCity();
                    break;

            }
            return sContent;
        }
        function GetCity() {
            return "北京";
        }
    </script>
</body>
</html>
