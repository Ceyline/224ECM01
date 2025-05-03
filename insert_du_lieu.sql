USE trangsucbac
GO

-- Insert data into Categories table
INSERT INTO DanhMuc (tenDanhMuc, moTa)
VALUES 
('Necklaces', 'Explore our exquisite collection of necklaces crafted with premium materials. From delicate chains to statement pieces, our necklaces are designed to complement any outfit and occasion. Each piece reflects timeless elegance and superior craftsmanship.'),
('Rings', 'Discover our stunning range of rings featuring various designs from classic bands to modern styles. Perfect for engagements, weddings, or everyday wear, our rings are made with high-quality metals and gemstones to last a lifetime.'),
('Earrings', 'Browse through our beautiful selection of earrings including studs, hoops, and dangling designs. Our earrings are hypoallergenic and comfortable to wear all day while adding a touch of sophistication to your look.'),
('Bracelets', 'Adorn your wrist with our elegant bracelet collection. From sleek bangles to charm bracelets, each piece is designed to be versatile and stylish. Perfect for layering or wearing alone as a statement piece.'),
('Wedding Jewelry', 'Our special wedding collection features bridal jewelry that will make your big day even more memorable. From traditional to contemporary designs, these pieces symbolize love and commitment with exceptional quality.');

-- Insert data into Products table
INSERT INTO SanPham (idDanhMuc, TenSanPham, GiaBan, HinhAnh, Size, SoLuong, MoTa)
VALUES 
-- Necklaces
(1, 'Diamond Pendant Necklace', 2500000, 'diamond-pendant.jpg', '16,18,20', 15, 'This stunning diamond pendant necklace features a brilliant-cut diamond set in premium 18K white gold. The delicate chain adds a touch of elegance perfect for both formal events and everyday wear.'),
(1, 'Pearl Strand Necklace', 1800000, 'pearl-strand.jpg', '17,19', 8, 'Classic and timeless, this pearl strand necklace features high-quality freshwater pearls with a sterling silver clasp. Perfect for adding a sophisticated touch to any outfit.'),
(1, 'Gold Plated Choker', 1200000, 'gold-choker.jpg', '14,15,16', 12, 'A modern take on the classic choker, this gold-plated necklace features a delicate design that sits beautifully on the neckline. Great for layering or wearing alone.'),

-- Rings
(2, 'Solitaire Diamond Ring', 3500000, 'solitaire-ring.jpg', '5,6,7,8', 10, 'The epitome of elegance, this solitaire diamond ring features a brilliant-cut center stone set in platinum. Perfect for engagements or as a special gift.'),
(2, 'Eternity Band', 2800000, 'eternity-band.jpg', '6,7,8,9', 7, 'This stunning eternity band features a continuous line of diamonds set in 14K white gold. Symbolizing never-ending love, it makes a perfect wedding or anniversary band.'),
(2, 'Stackable Gold Ring Set', 950000, 'stackable-rings.jpg', '5,6,7,8', 20, 'Mix and match these three delicate gold rings featuring different textures. Made from 14K gold, they''re perfect for everyday wear and can be stacked for a trendy look.'),

-- Earrings
(3, 'Diamond Stud Earrings', 2200000, 'diamond-studs.jpg', 'One Size', 15, 'Timeless diamond stud earrings set in 14K white gold. These classic earrings feature brilliant-cut diamonds that catch the light beautifully, suitable for any occasion.'),
(3, 'Hoop Earrings Set', 750000, 'hoop-set.jpg', 'Small,Medium,Large', 18, 'A versatile set of three gold-plated hoop earrings in different sizes. Wear them individually or layer them for a bold statement look.'),
(3, 'Pearl Drop Earrings', 1350000, 'pearl-drop.jpg', 'One Size', 9, 'Elegant pearl drop earrings featuring freshwater pearls suspended from delicate gold chains. Perfect for adding a touch of sophistication to formal attire.'),

-- Bracelets
(4, 'Tennis Bracelet', 3200000, 'tennis-bracelet.jpg', '6,7,8', 6, 'This exquisite tennis bracelet features a continuous line of diamonds set in 14K white gold. The secure clasp ensures safety while the design adds sparkle to your wrist.'),
(4, 'Charm Bracelet', 850000, 'charm-bracelet.jpg', '7,8', 14, 'Personalize this sterling silver charm bracelet with your choice of charms. The adjustable chain allows for a perfect fit, making it a great gift option.'),
(4, 'Bangle Set', 1100000, 'bangle-set.jpg', 'One Size', 11, 'A set of three gold-plated bangles with different textures. Wear them together for a layered look or individually for a subtle statement.'),

-- Wedding Jewelry
(5, 'Bridal Tiara', 1850000, 'bridal-tiara.jpg', 'One Size', 5, 'This stunning bridal tiara features delicate crystal detailing on a silver-plated base. Designed to complement wedding veils and add royal elegance to your special day.'),
(5, 'Wedding Band Set', 4200000, 'wedding-band-set.jpg', '6,7,8,9', 8, 'A matching set of wedding bands in 14K white gold with subtle diamond accents. Designed for both comfort and lasting beauty to symbolize your union.'),
(5, 'Bridal Pearl Necklace', 2100000, 'bridal-pearl.jpg', '16,18', 7, 'A luxurious pearl necklace designed specifically for brides. Featuring high-quality pearls with a diamond clasp, it adds timeless elegance to any wedding gown.');