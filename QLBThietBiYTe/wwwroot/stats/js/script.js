!(function () {
  "use strict";
  var o = window.location,
    r = window.document,
    t = r.currentScript,
    l = t.getAttribute("data-api") || new URL(t.src).origin + "/api/event",
    s = t.getAttribute("data-domain");
  t.hasAttribute("data-allow-fetch");
  function c(t, e, a) {
    e && console.warn("Ignoring Event: " + e), a && a.callback && a.callback();
  }
  function e(t, e) {
    if (
      /^localhost$|^127(\.[0-9]+){0,2}\.[0-9]+$|^\[::1?\]$/.test(o.hostname) ||
      "file:" === o.protocol
    )
      return c(0, "localhost", e);
    if (
      (window._phantom ||
        window.__nightmare ||
        window.navigator.webdriver ||
        window.Cypress) &&
      !window.__plausible
    )
      return c(0, null, e);
    try {
      if ("true" === window.localStorage.plausible_ignore)
        return c(0, "localStorage flag", e);
    } catch (t) {}
    var a,
      i,
      n = {};
    (n.n = t),
      (n.u = o.href),
      (n.d = s),
      (n.r = r.referrer || null),
      e && e.meta && (n.m = JSON.stringify(e.meta)),
      e && e.props && (n.p = e.props),
      (t = l),
      (n = n),
      (a = e),
      (i = new XMLHttpRequest()).open("POST", t, !0),
      i.setRequestHeader("Content-Type", "text/plain"),
      i.send(JSON.stringify(n)),
      (i.onreadystatechange = function () {
        4 === i.readyState &&
          a &&
          a.callback &&
          a.callback({
            status: i.status,
          });
      });
  }
  var a = (window.plausible && window.plausible.q) || [];
  window.plausible = e;
  for (var i, n = 0; n < a.length; n++) e.apply(this, a[n]);
  function p() {
    i !== o.pathname && ((i = o.pathname), e("pageview"));
  }
  function u() {
    p();
  }
  var w,
    t = window.history;
  t.pushState &&
    ((w = t.pushState),
    (t.pushState = function () {
      w.apply(this, arguments), u();
    }),
    window.addEventListener("popstate", u)),
    "prerender" === r.visibilityState
      ? r.addEventListener("visibilitychange", function () {
          i || "visible" !== r.visibilityState || p();
        })
      : p();
})();
