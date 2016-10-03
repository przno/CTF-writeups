// ** first to solve :) **

// does work only in few cases
// but to hack a system its enough to succeed once
// so repeat until successful! :)

// http://3a5yc7ypt0.hack-quest.com/
var india = function(text){

	var chars = ['a' , 'b' , 'c' , 'd' , 'e' , 'f' , 'g' , 'h' , 'i' , 'j' , 'k' , 'l' , 'm' , 'n' , 'o' , 'p' , 'q' , 'r' , 's' , 't' , 'u' , 'v' , 'w' , 'x' , 'y' , 'z' , 'A' , 'B' , 'C' , 'D' , 'E' , 'F' , 'G' , 'H' , 'I' , 'J' , 'K' , 'L' , 'M' , 'N' , 'O' , 'P' , 'Q' , 'R' , 'S' , 'T' , 'U' , 'V' , 'W' , 'X' , 'Y' , 'Z' , '0' , '1' , '2' , '3' , '4' , '5' , '6' , '7' , '8' , '9'];
	var coded = ['Ti', 'Hx', 'fJ', 'bl', 'gh', 'iI', 'Mo', 'Ho', '0f', 'ni', 'AJ', 'A8', 'Tx', 'gj', 'nG', 'Mt', 'Sj', '+i', 'ij', 'SG', 'eG', '0m', 'Ai', 'bi', 'i6', '+5', 'Lx', 'K4', 'fH', '+G', 'gJ', 'M6', 'ph', 'px', 'eH', 'Lj', 'Ay', 'TG', 'Mg', '08', 'Dk', 'Sm', 'nl', '+t', '0x', 'DH', 'b4', 'pf', 'Lm', 'KJ', 'g5', 'D6', 'fW', 'Hi', 'Hk', 'n6', 'Lo', 'Tl', 'Kf', 'DI', 'fG', 'S5'];

	var decoded = '';
	var found = true;

	text = text.substring(0, text.length - 6);

	for (var i = 0; i < text.length; i = i + 4){
		var couple = '' + text[i] + text[i + 1];

		for (var j = 0; j < coded.length; j++){
			if(coded[j] == couple){
				decoded += chars[j];
				break;
			}
			if(j === coded.length - 1){
				console.log('not found!');
				found = false;
			}
		}
	}

	if(found){
		console.log(decoded);
	}
}

// h4ck1t{th3_b3st_h@sh_3v3r}