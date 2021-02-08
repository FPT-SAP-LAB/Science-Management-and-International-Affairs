# FPT Science Management and International Affairs

[![CodeFactor](https://www.codefactor.io/repository/github/fpt-sap-lab/science-management-and-international-affairs/badge)](https://www.codefactor.io/repository/github/fpt-sap-lab/science-management-and-international-affairs)

## CÁC BƯỚC CHUẨN BỊ SAU KHI CLONE REPOS

### Tự động format code thừa sau khi push commit:
- Cài dotnet-format (nếu máy chưa cài): dotnet tool install -g dotnet-format
- Copy file `pre-commit` sang thư mục `.git\hooks`
## CONVENTION CHO GITHUB PULL REQUEST
* Pull Request sẽ chia làm các loại (TAG):</br>
        - <B>fix</B></br>
        - <B>add</B></br>
        - <B>remove</B></br>
        - <B>database</B></br>
        - <B>resources</B></br>
        - <B>config</B></br>
* Title của pull request sẽ được đặt theo format: <B>[<tên tag>]</B> - <I><Nội dung></I>
* Luôn Squashing Pull Requests giúp cho lịch sử commits được gọn

![image](https://29comwzoq712ml5vj5gf479x-wpengine.netdna-ssl.com/wp-content/uploads/2019/10/github-merge-options.png)

## CONVENTION CHO BACK-END
* Thêm Controller trực tiếp trong folder `Controllers`
* Tên Controller sẽ đặt theo PascalCase, ví dụ `HomeController`
* URL được mapping tự động theo `{controller}/{action}/{id}` nên tránh đặt tên Controller quá chung chung
* Khi tạo View thì `Add View` trực tiếp từ `return View();`
        
![image](https://user-images.githubusercontent.com/35557579/106367296-12574f00-6374-11eb-927f-65aa0cbc1203.png)

* Để thuận tiện cho việc quản lý source code thì tránh `[route()]` dưới mọi hình thức, đặt tên controller cẩn thận
* Luôn thêm `ViewBag.pagesTree` vào `Action`, không cần phải thêm cho trang chủ

![image](https://user-images.githubusercontent.com/35557579/107063796-5da6ad00-680d-11eb-88b9-7ba3ada76671.png)

## CONVENTION CHO FRONT-END
* Sau khi thêm đầy đủ ngôn ngữ ở `/Resources/*.resx`, thêm `System.Resources.ResourceManager rm = GUEST.Models.LanguageResource.GetResourceManager();` ở đầu file `.cshtml` và gọi resource theo cú pháp `rm.GetString("YourLabel")`

![image](https://user-images.githubusercontent.com/35557579/107229405-5f13e780-6a50-11eb-8993-a385b73bbe09.png)
