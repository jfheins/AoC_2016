temp = regexp(fileread('demo.txt'), '\r?\n', 'split');
mat = vertcat(temp{:});

walls = mat == '#';
emptySpace = mat == '.';
start = (mat =='0');
digits = (mat >='0');