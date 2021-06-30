using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Liyanjie.Utilities.Cn;

namespace System
{
    /// <summary>
    /// 
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 是否为手机号码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsPhoneNumber_Cn(this string input)
        {
            if (input == null || input == string.Empty || input.Length != 11)
                return false;

            return Regex.IsMatch(input, $"^{Consts.RegexPattern_PhoneNumber}$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 使用字符遮挡中间部分，只留前6位与后4位
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="char"></param>
        /// <returns></returns>
        public static string HidePhoneNumber_Cn(this string origin, char @char = '*')
        {
            return $"{origin.Substring(0, 3)}{@char}{@char}{@char}{@char}{origin.Substring(origin.Length - 4, 4)}";
        }

        /// <summary>
        /// 是否为身份证号
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsIdNumber_Cn(this string input)
        {
            if (string.IsNullOrWhiteSpace(input) || input.Length != 18) // 非18位为假 
                return false;

            if (!Regex.IsMatch(input, $"^{Consts.RegexPattern_IdNumber}$", RegexOptions.IgnoreCase))
                return false;

            var int1_17 = input.Substring(0, 17).ToCharArray().Select(m => int.Parse(m.ToString())).ToList();
            var code18 = input.Substring(17, 1);

            var verify = 0;
            var xxx = new int[] { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
            for (int i = 0; i < int1_17.Count; i++)
            {
                verify += int1_17[i] * xxx[i];
            }
            verify = verify % 11;

            var verifycode = string.Empty;
            switch (verify)
            {
                case 0:
                    verifycode = "1";
                    break;
                case 1:
                    verifycode = "0";
                    break;
                case 2:
                    verifycode = "X";
                    break;
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                    verifycode = (12 - verify).ToString();
                    break;
                default:
                    break;
            }

            if (string.IsNullOrWhiteSpace(verifycode))
                return false;

            if (!verifycode.Equals(code18, StringComparison.CurrentCultureIgnoreCase))  // 将身份证的第18位与算出来的校码进行匹配，不相等就为假 
                return false;

            return true;
        }

        /// <summary>
        /// 使用字符遮挡中间部分，只留前6位与后4位
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="char"></param>
        /// <returns></returns>
        public static string HideIdNumber_Cn(this string origin, char @char = '*')
        {
            return $"{origin.Substring(0, 6)}{@char}{@char}{@char}{@char}{origin.Substring(origin.Length - 4, 4)}";
        }

        /// <summary>
        /// 获取拼音
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static (string Pinyin, bool IsPinyin)[] GetPinyin(this string input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            if (input.Length == 0)
                return new (string Pinyin, bool IsPinyin)[0];

            return input.ToCharArray().Select(_ => GetPinyin(_)).ToArray();


            static (string Pinyin, bool IsPinyin) GetPinyin(char input)
            {
                if ((input > 47 && input < 58) || (input > 64 && input < 91) || (input > 96 && input < 123))
                    return (input.ToString(), false);

                if (PinyinHelper.TryGetPinyin(input, out var pinyins))
                    return (pinyins[0], true);

                return ("*", false);
            }
        }

        const string zhHans = "碍肮袄坝板办帮宝报币毙标表别卜补才蚕灿层搀谗馋忏缠偿厂彻尘衬惩迟冲丑出础处触辞聪丛担胆导灯邓敌籴递点淀电冬斗独吨夺堕儿矾范飞坟奋粪凤肤妇复复盖干干赶个巩沟构购谷顾刮关观柜汉号合轰后胡壶沪护划怀坏欢环还回伙获获击鸡积极际继家价艰歼茧拣硷舰姜浆桨奖讲酱胶阶疖洁借仅惊竞旧剧据惧卷开克垦恳夸块亏困腊蜡兰拦栏烂累垒类里礼隶帘联怜炼练粮疗辽了猎临邻岭庐芦炉陆驴乱么霉蒙蒙蒙梦面庙灭蔑亩恼脑拟酿疟盘辟苹凭扑仆朴启签千牵纤纤窍窃寝庆琼秋曲权劝确让扰热认洒伞丧扫涩晒伤舍沈声胜湿实适势兽书术树帅松苏苏虽随台台台态坛坛叹誊体粜铁听厅头图涂团团椭洼袜网卫稳务雾牺习系系戏虾吓咸显宪县响向协胁亵衅兴须悬选旋压盐阳养痒样钥药爷叶医亿忆应痈拥佣踊忧优邮余御吁郁誉渊园远愿跃运酝杂赃脏脏凿枣灶斋毡战赵折这征症证只只只致制钟钟肿种众昼朱烛筑庄桩妆装壮状准浊总钻爱罢备贝笔毕边宾参仓产长尝车齿虫刍从窜达带单当当党东动断对队尔发发丰风冈广归龟国过华画汇汇会几夹戋监见荐将节尽尽进举壳来乐离历历丽两灵刘龙娄卢虏卤卤录虑仑罗马买卖麦门黾难鸟聂宁农齐岂气迁佥乔亲穷区啬杀审圣师时寿属双肃岁孙条万为韦乌无献乡写寻亚严厌尧业页义艺阴隐犹鱼与云郑执质专嗳嫒叆瑷暧摆摆罴惫贞则负贡呗员财狈责厕贤账贩贬败贮贪贫侦侧货贯测浈恻贰贲贳费郧勋帧贴贶贻贱贵钡贷贸贺陨涢资祯贾损贽埙桢唝唢赅圆贼贿赆赂债赁渍惯琐赉匮掼殒勚赈婴啧赊帻偾铡绩溃溅赓愦愤蒉赍蒇赔赕遗赋喷赌赎赏赐赒锁馈赖赪碛㱮赗腻赛赘撄槚嘤赚赙罂镄箦鲗缨璎聩樱赜篑濑瘿懒赝豮赠鹦獭赞赢赡癞攒籁缵瓒臜赣趱躜戆滗荜哔筚跸笾傧滨摈嫔缤殡槟膑镔髌鬓渗惨掺骖毵瘆碜糁伧创沧怆苍抢呛炝玱枪戗疮鸧舱跄浐萨铲伥怅帐张枨胀涨鲿轧军轨厍阵库连轩诨郓轫轭匦转轮斩软浑恽砗轶轲轱轷轻轳轴挥荤轹轸轺涟珲载莲较轼轾辂轿晕渐惭皲琏辅辄辆堑啭崭裤裢辇辋辍辊椠辎暂辉辈链翚辏辐辑输毂辔辖辕辗舆辘撵鲢辙錾辚龀啮龆龅龃龄龇龈龉龊龌龋蛊诌邹驺绉皱趋雏苁纵枞怂耸撺镩蹿闼挞哒鞑滞郸惮阐掸弹婵禅殚瘅蝉箪蕲冁挡档裆铛谠傥镋冻陈岽栋胨鸫恸簖怼坠迩弥弥祢玺猕泼废拨䥽坺酦沣艳滟讽沨岚枫疯飒砜飓飔飕飗飘飙刚岗纲钢邝圹扩犷纩旷矿岿阄掴帼腘蝈挝哗骅烨桦晔铧婳刽郐侩浍荟哙狯绘烩桧脍鲙讥叽饥机玑矶虮郏侠陕浃挟荚峡狭惬硖铗颊蛱瘗箧刬浅饯线残栈盏钱笺践滥蓝尴槛褴篮苋岘觃视规现枧觅觉砚觇览宽蚬觊笕觋觌靓搅揽缆窥榄觎觏觐觑髋鞯蒋锵栉浕荩烬琎榉悫涞莱崃徕睐铼泺烁栎砾铄漓篱沥坜苈呖枥疬雳俪郦逦骊鹂酾鲡俩唡满瞒颟螨魉懑蹒棂浏陇泷宠庞垄拢茏咙珑栊昽胧砻袭聋龚龛笼偻溇蒌搂嵝喽缕屡数楼瘘褛窭镂屦蝼篓耧薮擞髅泸垆栌胪鸬颅舻鲈掳鹾箓滤摅论伦沦抡囵纶瘪萝啰逻猡椤锣箩冯驭闯吗犸驮驰驯妈玛驱驳码驼驻驵驾驿驷驶驹骀驸驽骂蚂笃骇骈骁骄骆骋验骏骎骑骐骒骓骗骘骛骚骞骜蓦腾骝骟骠骢骡羁骤骥骧荬读渎续椟犊牍窦黩唛麸闩闪们闭问扪闱闵闷闰闲间闹闸钔阂闺闻闽闾闿阁阀润涧悯阆阅阃娴阏阈阉阊阍阌阋阎焖阑裥阔痫鹇阕阒搁锏锎阙阖阗榈简谰阚蔺澜斓镧躏渑绳鼋蝇鼍傩滩摊瘫凫鸠岛茑鸢鸣枭鸩鸦鸥鸨窎莺鸪捣鸭鸯鸮鸲鸰鸳鸵袅鸱鸶鸾鸿鸷鸸鸼鸽鸹鸺鸻鹈鹁鹃鹆鹄鹅鹑鹒鹉鹊鹋鹌鹏鹐鹚鹕鹖鹗鹘鹙鹜鹛鹤鹣鹞鹡䴙䴘鹧鹥鹨鹫鹩鹪鹬鹰鹯鹭鹳鸤䴙䴘慑滠摄嗫镊颞蹑泞拧咛狞柠聍侬浓哝脓剂侪济荠挤脐蛴跻霁鲚齑剀凯恺垲桤硙皑铠忾饩跹剑俭险捡猃检殓敛脸裣睑签潋蔹侨挢荞峤娇桥硚矫鞒榇䓖讴伛沤怄抠奁呕岖妪枢瓯欧殴眍躯蔷墙嫱樯穑铩谉婶柽蛏浉狮蛳筛埘莳鲥俦涛祷焘畴铸筹踌嘱瞩萧啸潇箫蟏刿哕秽荪狲逊涤绦鲦厉迈励疠虿趸砺粝蛎伪沩妫讳伟违苇韧帏围纬炜祎玮韨涠韩韫韪韬邬坞呜钨怃庑抚芜呒妩谳芗飨泻浔荨挦鲟垩垭挜哑娅恶恶氩俨酽恹厣靥餍魇黡侥浇挠荛峣哓娆绕饶烧桡晓硗铙翘蛲跷邺顶顷项顸顺须颃烦顼顽顿颀颁颂倾预庼硕领颈颇颏颉颍颌颋滪颐蓣频颓颔颖颗额颜撷题颙颛缬濒颠颡嚣颢颤巅颥癫灏颦颧议仪蚁呓荫瘾莸鱽渔鲂鱿鲁鲎蓟鲆鲏鲅鲇鲊䲟稣鲋鲍鲐鲞鲝鲛鲜鲑鲒鲔鲖鲨噜鲠鲫鲩鲣鲤鲧橹氇鲸鲭鲮鲰鲲鲻鲳鲱鲵鲷鲶藓䲡䲠鳊鲽鳁鳃鳄镥鳅鳆鳇鳌䲢鳒鳍鳎鳏鳑癣鳖鳙鳛鳕鳔鳓鳘鳗鳝鳟鳞鳜鳣鳢屿欤芸昙叇掷踯垫挚蛰絷锧踬传抟砖计订讣讨讧讦记讯讪训讫访讶讵诀讷设讹许讼讻诂诃评诏词译诎诇诅识诋诉诈诊诒该详诧诓诖诘诙试诗诩诤诠诛诔诟诣话诡询诚诞浒诮说诫诬语诵罚误诰诳诱诲狱谊谅谈谆谇请诺诸诼诹课诽诿谁谀调谄谂谛谙谜谚谝谘谌谎谋谍谐谏谞谑谒谔谓谖谕谥谤谦谧谟谡谢谣储谪谫谨谬谩谱谮谭谲谯蔼槠谴谵辩䜩雠谶霭饦饧饨饭饮饫饪饬饲饰饱饴饳饸饷饺饻饼饵蚀饹饽馁饿馆馄馃馅馉馇馊馐馍馎馏馑馒馓馔馕汤扬场旸炀杨肠疡砀畅钖殇荡烫觞丝纠纡纣红纪纫纥约纨级纺纹纭纯纰纽纳纱纴纷纸纾纼咝绊绀绁绂绋绎经绍组细䌷绅织绌终绐哟绖荮荭绞统绒绔结绗给绝绛络绚绑莼绠绨绡绢绣绥综绽绾绻绫绪绮缀绿绰绲绯绶绸绷绺维绵缁缔编缃缂缅缘缉缇缈缗缊缌缓缄缑缒缎缞缟缣缢缚缙缛缜缝缡潍缩缥缪缦缫缧蕴缮缯缭橼缰缳缲缱缴辫坚肾竖悭紧铿劳茕茔荧荣荥荦涝崂莹捞唠萤营萦痨嵘铹耢蝾览鉴帜炽职钆钇钌钋钉针钊钗钎钓钏钍钐钒钖钕钬钫钚䥺钪钯钭钙钝钛钘钮钞钠钤钧钩钦铋钰钲钳钴钺钵钹钼钾铀钿铎钹铃铅铂铆铍钶铊钽铌钷铈铉铒铑铕铟铷铯铥铪铞铫铵衔铰铳铱铓铐铏银铜铝铨铢铣铤铭铬铮揿锌锐锑锒铺嵚锓锃销锄锅锉锈锋锆锔锕铽锇锂锘锞锭锗锝锫错锚锛锯锰锢锟锡锤锥锦锨锱键镀镃镁锲锷锶锴锾锹锿镅锻锸锼镎镓镒镑镐镉镇镍镌镏镜镝镛镞镖镚镗镘镦镨镤镢镣镫镪镰镱镭镬镮镯镲镳镴镶镢峃学喾黉泽怿择峄萚释箨劲刭陉泾茎径烃氢胫痉羟巯变弯孪峦娈恋栾挛鸾湾蛮脔滦銮剐涡埚莴娲祸脶窝蜗呆呆布痴床唇雇挂哄哄迹迹秸杰巨昆昆捆泪厘麻脉猫栖弃升升笋它席凶绣锈岩异涌岳韵灾札札扎扎占周注";
        const string zhHant = "礙骯襖壩闆辦幫寶報幣斃標錶彆蔔補纔蠶燦層攙讒饞懺纏償廠徹塵襯懲遲衝醜齣礎處觸辭聰叢擔膽導燈鄧敵糴遞點澱電鼕鬥獨噸奪墮兒礬範飛墳奮糞鳳膚婦復複蓋乾幹趕個鞏溝構購榖顧颳關觀櫃漢號閤轟後鬍壺滬護劃懷壞歡環還迴夥獲穫撃鷄積極際繼傢價艱殲繭揀鹸艦薑漿槳奬講醤膠階癤潔藉僅驚競舊劇據懼捲開剋墾懇誇塊虧睏臘蠟蘭攔欄爛纍壘類裏禮隷簾聯憐煉練糧療遼瞭獵臨鄰嶺廬蘆爐陸驢亂麽黴矇濛懞夢麵廟滅衊畝惱腦擬釀瘧盤闢蘋憑撲僕樸啓籤韆牽縴纖竅竊寢慶瓊鞦麯權勸確讓擾熱認灑傘喪掃澀曬傷捨瀋聲勝濕實適勢獸書術樹帥鬆蘇囌雖隨臺檯颱態壇罎嘆謄體糶鐵聽廳頭圖塗團糰橢窪襪網衛穩務霧犧習係繫戲蝦嚇鹹顯憲縣響嚮協脅褻釁興鬚懸選鏇壓鹽陽養癢様鑰藥爺葉醫億憶應癰擁傭踴憂優郵餘禦籲鬱譽淵園遠願躍運醖雜臓贜髒鑿棗竈齋氈戰趙摺這徵癥證隻祗衹緻製鐘鍾腫種衆晝硃燭築莊樁妝裝壯狀凖濁總鑽愛罷備貝筆畢邊賓參倉産長嘗車齒蟲芻從竄達帶單當噹黨東動斷對隊爾發髮豐風岡廣歸龜國過華畫匯彙會幾夾戔監見薦將節盡儘進舉殻來樂離歷曆麗兩靈劉龍婁盧虜鹵滷録慮侖羅馬買賣麥門黽難鳥聶寜農齊豈氣遷僉喬親窮區嗇殺審聖師時夀屬雙肅嵗孫條萬為韋烏無獻鄉寫尋亞嚴厭堯業頁義兿陰隱猶魚與雲鄭執質專噯嬡靉璦曖擺襬羆憊貞則負貢唄員財狽責厠賢賬販貶敗貯貪貧偵側貨貫測湞惻貳賁貰費鄖勛幀貼貺貽賤貴鋇貸貿賀隕溳資禎賈損贄塤楨嗊嗩賅圓賊賄贐賂債賃漬慣瑣賚匱摜殞勩賑嬰嘖賒幘僨鍘績潰濺賡憒憤蕢賫蕆賠賧遺賦噴賭贖賞賜賙鎖饋賴赬磧殨賵膩賽贅攖檟嚶賺賻罌鐨簀鰂纓瓔聵櫻賾簣瀨癭懶贋豶贈鸚獺贊贏贍癩攢籟纘瓚臢贛趲躦戇潷蓽嗶篳蹕籩儐濱擯嬪繽殯檳臏鑌髕鬢滲慘摻驂毿瘮磣穇糝傖創滄愴蒼搶嗆熗瑲槍戧瘡鶬艙蹌滻薩鏟倀悵帳張棖脹漲鱨軋軍軌厙陣庫連軒諢鄆軔軛匭轉輪斬軟渾惲硨軼軻軲軤輕轤軸揮葷轢軫軺漣琿載蓮較軾輊輅轎暈漸慚皸璉輔輒輛塹囀嶄褲褳輦輞輟輥槧輜暫輝輩鏈翬輳輻輯輸轂轡轄轅輾輿轆攆鰱轍鏨轔齔嚙齠齙齟齡齜齦齬齪齷齲蠱謅鄒騶縐皺趨雛蓯縱樅慫聳攛鑹躥澾闥撻噠韃滯鄲憚闡撣彈嬋禪殫癉蟬簞蘄囅擋檔襠鐺讜儻钂凍陳崬棟腖鶇慟籪懟墜邇彌瀰禰壐獼潑廢撥鏺墢醱灃艶灧諷渢嵐楓瘋颯碸颶颸颼飀飄飆剛掆崗綱棡鋼鄺壙擴獷纊曠礦巋鬮摑幗膕蟈撾嘩驊燁樺曄鏵嫿擓劊鄶儈澮薈噲獪繪燴檜膾鱠譏嘰饑機璣磯蟣郟俠陝浹挾莢峽狹愜硤鋏頰蛺瘞篋剗淺餞綫殘棧盞錢箋踐濫藍尷檻襤籃莧峴覎視規現梘覓覺硯覘覽寬蜆覬筧覡覿靚攪攬纜窺欖覦覯覲覷髖韉蔣鏘櫛濜藎燼璡櫸慤淶萊峽徠睞錸濼爍櫟礫鑠灕籬瀝壢藶嚦櫪癧靂儷酈邐驪鸝釃鱺倆啢滿瞞顢蟎魎懣蹣欞瀏隴瀧寵龐壟攏蘢嚨瓏櫳龑曨朧礱襲聾龔龕籠僂漊蔞摟嶁嘍縷屢數樓瘻褸窶瞜鏤屨螻簍耬藪擻髏濾壚櫨臚鸕顱艫鱸擄鹺籙濾攄論倫淪掄圇綸癟蘿囉邏玀欏鑼籮馮馭闖嗎獁馱馳馴媽瑪驅駁碼駝駐駔駕驛駟駛駒駘駙駑駡螞篤駭駢驍驕駱騁驗駿駸騎騏騍騅騙騭騖騷騫驁驀騰騮騸驃驄騾覊驟驥驤蕒讀瀆續櫝犢牘竇黷嘜麩閂閃們閉問捫闈閔悶閏閑間閙閘鍆閡閨聞閩閭闓閣閥潤澗憫閬閲閫嫻閼閾閹閶閽閿鬩閻燜闌襇闊癇鷳闋闃擱鐧鐦闕闔闐櫚簡讕闞藺瀾斕鑭躪澠繩黿蠅鼉儺灘擹癱鳬鳩島蔦鳶鳴梟鴆鴉鳾鷗鴇窵鶯鴣搗鴨鴦鴞鴝鴒鴛鴕裊鴟鷥鵉鵁鴻鷙鴯鴷鵃鴿鴰鵂鴴鵜鵓鵑鵒鵠鵝鶉鶊鶄鵡鵲鶓鵪鵬鵮鷀鶘鶡鶪鶚鶻鶖鶩鶥鶴鶼鷂鶺鸊鷉鷓鷖鷚鹫鷯鷦鷸鷹鸇鷺鸊鸛鳲𪁉𪂴鷿鷈攝灄攝囁鑷顳躡濘擰嚀獰檸聹儂濃噥膿劑儕濟薺擠臍蠐蠐霽鱭齏剴凱愷塏榿磑皚鎧愾餼躚劍儉險撿獫檢殮斂臉襝瞼簽瀲蘞僑撟蕎嶠嶠橋礄矯鞽櫬藭謳傴漚慪摳奩嘔嶇嫗樞甌歐毆瞘軀薔墻嬙檣穡鎩讅嬸檉蟶溮獅螄篩塒蒔鰣儔濤禱燾疇鑄籌躊囑矚蕭嘯瀟簫蠨劌噦穢蓀猻遜滌縧鰷厲邁勵癘蠆躉礪糲蠣僞溈媯諱偉違葦韌幃圍緯煒禕瑋韍潿韓韞韙韜鄔塢嗚鎢憮廡撫蕪嘸嫵讞薌饗瀉潯蕁撏鱘堊埡掗啞婭惡噁氬儼釅懨厴靨饜魘黶僥澆撓蕘嶢嘵嬈繞饒焼橈曉磽鐃翹蟯蹺鄴頂頃項頇順須頏煩瑣頑頓頎頒頌傾預廎碩領頸頗頦頡潁頜頲澦頤蕷頻頽頷穎顆額顔擷題顒顓纈瀕顛顙囂顥顫巔顬癲灝顰顴議儀蟻囈蔭癮蕕魛漁魴魷魯鱟薊鮃鮍鮁點鮓鮣穌鮒鮑鮐鯗鮺鮫鮮鮭鮚鮪鮦鯊嚕鯁鯽鯇鰹鯉鯀櫓氌鯨鯖鯪鯫鯤鯔鯧鯡鯢鯛鯰蘚鰌鰆鯿鰈鰛鰓鰐鑥鰍鰒鰉鰲鰧鰜鰭鰨鰥鰟癬鱉鱅鰼鱈鰾鰳鰵鰻鱔鱒鱗鱖鱣鱧嶼歟蕓曇靆擲躑墊摯蟄縶鑕躓傳摶膞磚計訂訃討訌訐記訊訕訓訖訪訝詎訣訥設訛訢許訟訩詁訶評詔詞譯詘詗詛識詆訴詐診詒該詳詫誆詿詰詼試詩詡諍詮誅誄詬詣話詭詢誠誕滸誚説誡誣語誦罸誤誥誑誘誨獄誼諒談諄誶請諾諸諑諏課誹諉誰諛調諂諗諦諳謎諺諞諮諶謊謀諜諧諫諝謔謁諤謂諼諭謚謗謙謐謨謖謝謡儲謫譾謹謬謾譜譖譚譎譙藹櫧譴譫辯讌讎讖靄飥餳飩飯飲飫飪飭飼飾飽飴飿餄餉餃餏餠餌蝕餎餑餒餓館餛餜餡餶餷餿饈饃餺餾饉饅饊饌饟湯揚場暘煬楊腸瘍碭暢錫殤蕩燙觴絲糾紆紂紅紀紉紇约紈級紡紋紜純紕紐納紗紝紛紙紓紖噝絆紺紲紱紼繹經紹組細紬紳織絀終紿喲絰葤葒絞統絨絝結絎給絶絳絡絢綁蒓綆綈綃絹綉綏綜綻綰綣綾緒綺綴緑綽緄緋綬綢綳綹維綿緇締編緗緙緬緣緝緹緲緡緼緦緩緘緱縋緞縗縞縑縊縛縉縟縝縫縭濰縮縹繆縵繅縲藴繕繒繚櫞繮繯繰繾繳辮堅腎竪慳緊鏗勞煢塋熒榮滎熒澇嶗瑩撈嘮螢營縈癆嶸鐒耮蠑覧鑒幟熾職釓釔釕釙釘針釗釵釺釣釧釷釤釩鍚釹鈥鈁鈈釾鈧鈀鈄鈣鈍鈦鈃鈕鈔鈉鈐鈞鈎欽鉍鈺鉦鉗鈷鉞鉢鈸鉬鉀鈾鈿鐸鏺鈴鉛鉑鉚鈹鈳鉈鉭鈮鉕鈰鉉鉺銠銪銦銣銫銩鉿銱銚銨銜鉸銃銥鋩銬鉶銀銅鋁銓銖銑鋌銘鉻錚撳鋅鋭銻鋃鋪嶔鋟鋥銷鋤鍋銼銹鋒鋯鋦錒鋱鋨鋰鍩錁錠鍺鍀錇錯錨錛鋸錳錮錕錫錘錐錦鍁錙鍵鍍鎡鎂鍥鍔鍶鍇鍰鍬鎄鎇鍛鍤鎪鎿鎵鎰鎊鎬鎘鎮鎳鎸鎦鏡鏑鏞鏃鏢鏰鏜鐯鏝鐓鐥鐠鏷鐝鐐鐙鏹鐮鐿鐳鑊鐶鐲鑔鑣鑞鑲钁嶨學嚳黌澤懌擇嶧蘀釋籜勁剄陘涇莖徑烴氫脛痙羥巰變彎孿巒孌戀欒攣鸞灣蠻臠灤鑾剮渦堝喎萵媧禍腡窩蝸獃騃佈癡牀脣僱掛閧鬨跡蹟稭傑鉅崑崐綑淚釐蔴脈貓棲棄阩昇筍牠蓆兇繡鏽巖異湧嶽韻灾剳劄紥紮佔週註";

        /// <summary>
        /// 繁转简
        /// </summary>
        /// <param name="zhHantInput"></param>
        /// <returns></returns>
        public static string ChangeToZHHans(this string zhHantInput)
        {
            var builder = new StringBuilder(zhHantInput.Length);
            foreach (var item in zhHantInput)
            {
                var index = zhHant.IndexOf(item);
                builder.Append(index < 0 ? item : zhHans[index]);
            }
            return builder.ToString();
        }

        /// <summary>
        /// 简转繁
        /// </summary>
        /// <param name="zhHansInput"></param>
        /// <returns></returns>
        public static string ChangeToZHHant(this string zhHansInput)
        {
            var builder = new StringBuilder(zhHansInput.Length);
            foreach (var item in zhHansInput)
            {
                var index = zhHans.IndexOf(item);
                builder.Append(index < 0 ? item : zhHant[index]);
            }
            return builder.ToString();
        }
    }
}
