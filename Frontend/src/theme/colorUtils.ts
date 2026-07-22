export const getAdaptedRoleColor = (hexColor: string | undefined | null, isDarkMode: boolean): string => {
  if (!hexColor || hexColor.trim() === '') return 'text.primary';
  if (!isDarkMode) return hexColor;

  let hex = hexColor.replace('#', '');
  if (hex.length === 3) {
    hex = hex.split('').map(c => c + c).join('');
  }
  if (hex.length !== 6) return hexColor;

  const r = parseInt(hex.substring(0, 2), 16) / 255;
  const g = parseInt(hex.substring(2, 4), 16) / 255;
  const b = parseInt(hex.substring(4, 6), 16) / 255;

  const max = Math.max(r, g, b);
  const min = Math.min(r, g, b);
  let h = 0;
  let s = 0;
  let l = (max + min) / 2;

  if (max !== min) {
    const d = max - min;
    s = l > 0.5 ? d / (2 - max - min) : d / (max + min);
    switch (max) {
      case r: h = (g - b) / d + (g < b ? 6 : 0); break;
      case g: h = (b - r) / d + 2; break;
      case b: h = (r - g) / d + 4; break;
    }
    h /= 6;
  }

  const hDeg = h * 360;
  const sPct = s * 100;
  const lPct = l * 100;

  let targetL = 100 - lPct;

  if (targetL < 76) targetL = 76;
  if (targetL > 86) targetL = 86;

  let targetS = sPct;

  if (targetS > 65) targetS = 65;
  if (targetS < 40) targetS = 40;

  const lDec = targetL / 100;
  const sDec = targetS / 100;
  const a = sDec * Math.min(lDec, 1 - lDec);

  const f = (n: number) => {
    const k = (n + hDeg / 30) % 12;
    const color = lDec - a * Math.max(Math.min(k - 3, 9 - k, 1), -1);
    return Math.round(255 * color).toString(16).padStart(2, '0');
  };

  return `#${f(0)}${f(8)}${f(4)}`;
};