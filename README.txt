Controllerlar üzerinden proje detayı
1-Login;
Identitynin özelliği olan userManager ve SignInManager kullanılarak kullanıcı oluşturuyor.
Yazılan DTO'dan dönen mail var olan maillerle karşılaştırılıyor. Eğer Nesne doluysa Önce bir kez signout yani
çıkış yaptırılıyor. Sonra SignInManager'ın özelliği olan PasswordSignInAsync metoduyla giriş yaptırılıyor.
Başarılı ise Anasayfa'ya yönlendirme gerçekleşiyor. 
Başarısız ise kullanıcının giriş denemeleri arttırılıyor ve E-posta veya şifre yanlış hatası dönüyor. 3 kez yanlış giriş yaparsa, 1 dk lık lockdown, yani hesap
kilitleniyor.
Register kısmında ise bir adet appuser nesnesi yaratılıyor ve CreateAsync metoduyla kullanıcı kaydı yapılıyor. AddToRoleAsync metoduyla da rol kaydı
gerçekleştiriliyor.
Daha sonra ise Confirmation Metoduna appuser nesnesiyle birlikte yönlendirme yapılıyor.
Confirmation metodunda ConfirmModel adında oluşturulan DTO nesnesine 6 haneli bir random sayı tanımlanıyor.
Ve smtp işlemleriyle bu kod kullanıcı mail ine gönderiliyor.
Bu kod girildiğinde kullanıcının Database deki emailconfirmed bloğu true ya çeviriliyor.
Logout ile de çıkış işlemi gerçekleştiriliyor.

2-Home;
Bu controller da default olarak view kısmı zaten kapalı, normal üye olmayanlar girdiğinde sadece indexin bir kısmını görecekler,
fakat user authentice olduğunda index kısmı değişmiş olacak, ve controllerın diğer metodlarına erişebilir olacak.
Entry Metodu;
Öncelikle herhangi bir kategori oluşturmadıysa kendine özgü, bu metod kendini createcategory metoduna yönlendiricek,
CreateCategory metoduyla user kendine bir kategori oluşturabilir.
Herhangi bir kategorisi var ise userın entry metodunda kendine bir giriş yapabilir.
Propların özelliğine göre gider veya gelir olarak gösterecek.
Belli bir miktar giricek.
Girdiği para tipini seçicek Nakit veya kredikartı.
Girilen tiplere göre AppUser içindeki değerler güncelleniyor.
Balance,cash,creditdebt vs..
Cash ve Cashminus metodları ile elden nakit girişi veya çıkışı yapabilir.
Credit ve CreditMinus metodları ile elden kredikartı borcu girişi ve çıkışı yapabilir.
BankAction metodu ile de nakit ile kredikartı borcu arasında takas yapabilecek.
Saver metodu ile Sanal Kumbarasına giriş yapabilir, tipine göre Kumbaradan para alabilir veya para ekleyebilir.
Son Olarak CreateMonthly metodu ile taksitli veya aylık işlem ekleniyor. Tipine göre taksitli ise otomatik olarak aylık 
günü geldiğinde giriş yapıyor. Sınırsız ise de aynı şekilde aylık günü geldiğinde giriş yapıyor.
Örneğin netflix aboneliği gider olarak kredikartından ödeniyor gösterilirse her ay günü geldiğinde kredikartı borcuna otomatik olarak ekleme yapılacak
Aynı şekilde Maaş vs. bir Gelir gösterilirse her ay bakiyeye ve entrylere otomatik olarak ekleniyor gösterilicek.

3-Progress;
Database deki bütün girdileri bir ProgressDTO üzerinden toplanıyor ve userid si aynı olanlar seçiliyor.
Progress controller ı kullanıcının yaptığı harcamaları vs. genel olarak görebilmesine olanak sağlıyor.
Girdiği aylık işlemler, tekli işlemler, kumbaraya girip çıktığı işlemler vs.. hepsini bu kısımdan görebiliyor.
Özellikle aynı kategoriye ait olup,"200 lira altında" olan birden fazla gider varsa bunları da listeliyor.
Böylelikle küçük bazlı görünüp cepten çıkan görünmesi zor olan giderler takip edilebiliyor.
Aynı zamanda taksitli veya taksitsiz işlemlerini görebilirken, kalan taksit sayısı vs.. şeklinde ayrılan işlemlerini görebiliyor.


ADMIN AREA;
1-Home;
Herhangi bir metod tanımlanmadı sadece view kısmında kullanıcı için hoşgeldiniz yazısı var.
2-Info;
Info controllerında kullanıcı için bilgileri yer almaktadır.
Şifre değişikliği yapabilir, kullanıcı adı değiştirebilir.
Kredi Kartı kesim gününü değiştirebilir ve faizinin çalışıp çalışmamasını yani günü geldiğinde otomatik olarak faizin
kredi kartının faiz oranıyla çarpılıp çarpılmamasını etkinleştirebilir.
3-Category;
Bu controllerda tipik CRUD işlemleri bulunmakta kullanıcı kendi kategorilerini görebilir silebilir ekleyebilir güncelleyebilir.
4-Entry;
Bu controllerda tipik CRUD işlemleri bulunmakta kullanıcı kendi entrilerini görebilir silebilir ekleyebilir güncelleyebilir.
5-Saves;
Bu controllerda tipik CRUD işlemleri bulunmakta kullanıcı kendi savelerini görebilir silebilir ekleyebilir güncelleyebilir.
6-Monthly;
Bu controllerda tipik CRUD işlemleri bulunmakta kullanıcı kendi aylık işlemlerini görebilir silebilir ekleyebilir güncelleyebilir.
7-Saving;
Bu controllerda ise kişi kendisine bir hedef belirlemek için işlem gerçekleştirebilir.
Tarihine göre belirlenen hedefe yaklaşmakta kullanılabilir.